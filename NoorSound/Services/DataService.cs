// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\

using NoorSound.Models;
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
        
        
        public async Task AddAudio(AudioInsert audio)
        {
            await _supabaseClient.From<AudioInsert>().Insert(audio);
            
        }

        public async Task<IEnumerable<Audio>> GetAudios()
        {
            var response = await _supabaseClient.From<Audio>().Get(); // In Shaa Allah, get all audios from Audio table 
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
            // Save the Image url and Audio url from Supabase Storage
            var audio = await _supabaseClient.From<Audio>().Where(a => a.Id == id).Single();

            if(audio == null)
            {
                return;
            }

            // In Shaa Allah ta'ala, first deleting the Image url and Audio url from Supabase Storage
            if (!string.IsNullOrWhiteSpace(audio.ImagePath))
            {
                await _supabaseClient.Storage.From("images").Remove(new List<string>() { audio.ImagePath });
            }

            if (!string.IsNullOrWhiteSpace(audio.AudioPath))
            {
                await _supabaseClient.Storage.From("audio-files").Remove(new List<string>() { audio.AudioPath});
            }


            // then deleting the audio row from the database
            await _supabaseClient.From<Audio>().Where(a => a.Id == id).Delete();
        }



        
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