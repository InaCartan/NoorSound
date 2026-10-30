// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\

using NoorSound.Models;
using NoorSound.Services.Interfaces;
using Supabase;



namespace NoorSound.Services
{

    // * TODO: Check if this file is like a repository. *
    public class DataService : IDataService
    {
        private readonly Client _supabaseClient;

        
        public DataService(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }



        // ------- Audio ------- 

        public async Task<long> AddAudio(AudioInsert audio)
        {
            var response = await _supabaseClient.From<AudioInsert>().Insert(audio);
            return response.Models.First().Id;
            
        }

        public async Task<IEnumerable<Audio>> GetAudios()
        {
            // In Shaa Allah ta'ala, get all audios from Audio table 
            var response = await _supabaseClient.From<Audio>().Get();

            return response.Models.OrderByDescending(a => a.MadeAt);
        }


        public async Task UpdateAudio(Audio audio)
        {
            await _supabaseClient.From<Audio>().Where(a => a.Id == audio.Id) 
                .Set(a => a.AudioName, audio.AudioName)
                .Set(a => a.ImageUrl, audio.ImageUrl)
                .Set(a => a.ImagePath, audio.ImagePath)
                .Set(a => a.AudioUrl, audio.AudioUrl)
                .Set(a => a.AudioPath, audio.AudioPath)
                .Update();
        }

        public async Task DeleteAudio(long id)
        {
            // Saves the Image url and Audio url from Supabase Storage
            var audio = await _supabaseClient.From<Audio>().Where(a => a.Id == id).Single();

            if(audio == null)
            {
                return;
            }

            // In Shaa Allah ta'ala, first deleting the audio row from the database
            await _supabaseClient.From<Audio>().Where(a => a.Id == id).Delete();


            // then deleting the Image url and Audio url from Supabase Storage
            if (!string.IsNullOrWhiteSpace(audio.ImagePath))
            {
                await _supabaseClient.Storage.From("images").Remove(new List<string>() { audio.ImagePath });
            }

            if (!string.IsNullOrWhiteSpace(audio.AudioPath))
            {
                await _supabaseClient.Storage.From("audio-files").Remove(new List<string>() { audio.AudioPath});
            }
        }


        // ------- Playlist ------- 

        public async Task<long> AddPlaylist(PlaylistInsert playlist)
        {
            var response = await _supabaseClient.From<PlaylistInsert>().Insert(playlist);
            return response.Models.First().Id;
        }


        public async Task AddAudiosToPlaylist(IEnumerable<PlaylistAudioInsert> playlistAudios)
        {
            await _supabaseClient.From<PlaylistAudioInsert>().Insert(playlistAudios.ToList());
        }


        public async Task<IEnumerable<Audio>> GetAudiosInPlaylist(long playlistId)
        {
            // 1) The junction rows of this playlist
            var junction = await _supabaseClient.From<PlaylistAudio>()
                .Where(pa => pa.PlaylistId == playlistId)
                .Get();

            var audioIds = junction.Models.Select(pa => pa.audioId).ToList();

            if (audioIds.Count == 0)
            {
                return Enumerable.Empty<Audio>();
            }

            // 2) The audios themselves
            var audios = await _supabaseClient.From<Audio>()
                .Filter("id", Postgrest.Constants.Operator.In, audioIds)
                .Get();

            // Keep the order in which they were added to the playlist
            var audioById = audios.Models.ToDictionary(a => a.Id);

            return junction.Models
                .OrderBy(pa => pa.MadeAt)
                .Where(pa => audioById.ContainsKey(pa.audioId))
                .Select(pa => audioById[pa.audioId])
                .ToList();
        }

        public async Task DeletePlaylist(long id)
        {
            await _supabaseClient.From<Playlist>().Where(p => p.Id == id).Delete();
        }


        // ------- Playlist (junction table) ------- 

        public async Task<IEnumerable<Playlist>> GetPlaylists()
        {
            var response = await _supabaseClient.From<Playlist>().Get();

            return response.Models.OrderByDescending(plst => plst.MadeAt);
        }

        public async Task<IEnumerable<PlaylistAudio>> GetPlaylistAudios()
        {
            var response = await _supabaseClient.From<PlaylistAudio>().Get();

            return response.Models.OrderByDescending(plstAudio => plstAudio.MadeAt);
        }



        // ------- Admin ------- 
        public async Task<Admin?> GetAdmin(string id)
        {
            return await _supabaseClient.From<Admin>()
                .Where(admin => admin.Id == id)
                .Single();
        }

        public async Task<IEnumerable<Audio>> GetAudiosByAdmin(string adminId)
        {
            var response = await _supabaseClient.From<Audio>()
                .Where(a => a.AdminId == adminId)
                .Get();

            return response.Models.OrderByDescending(a => a.MadeAt);
        }


        // ------- Storage ------- 

        public async Task DeleteFileFromStorage(string bucket, string path)
        {

            if (string.IsNullOrWhiteSpace(bucket))
            {
                throw new ArgumentException("Storage bucket is required");
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            await _supabaseClient.Storage.From(bucket).Remove(new List<string>() { path });
        }

        // TODO: Change function, so it have Resumable and Streamed uploading (using the TUS protocol)
        public async Task<(string Path, string PublicUrl)> UploadFile(Stream fileStream, string fileName, string bucket)
        {
            var path = $"{Guid.NewGuid()}_{fileName}";

            // Converting Stream to byte array
            // In Shaa Allah, hover over the code to see explanations
            using var memoryStram = new MemoryStream();
            await fileStream.CopyToAsync(memoryStram);
            

            // In Shaa Allah, this will uploade the file to Supabase
            await _supabaseClient.Storage.From(bucket).Upload(memoryStram.ToArray(), path);

            // In Shaa Allah, getting the public URL of the uploaded file
            var publicUrl = _supabaseClient.Storage.From(bucket).GetPublicUrl(path);

            return (path, publicUrl);
        }


        
    }
}