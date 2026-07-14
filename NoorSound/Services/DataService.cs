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
                .Set(a => a.AudioUrl, audio.AudioUrl)
                .Update();
        }

        public async Task DeleteAudio(int id)
        {
            // Save the Image url and Audio url from Supabase Storage
            var audio = await _supabaseClient.From<Audio>().Where(a => a.Id == id).Single();

            if(audio == null)
            {
                return;
            }

            // Delete the Image url and Audio url from Supabase Storage
            if (!string.IsNullOrWhiteSpace(audio.ImageUrl))
            {
                await _supabaseClient.Storage.From("images").Remove(new List<string>() { audio.ImageUrl });
            }

            if (!string.IsNullOrWhiteSpace(audio.AudioUrl))
            {
                await _supabaseClient.Storage.From("audio-files").Remove(new List<string>() { audio.AudioUrl});
            }


            // In Shaa Allah ta'ala, now deleting the audio row from the database
            // (after having deleting the image and audio files) 
            await _supabaseClient.From<Audio>().Where(a => a.Id == id).Delete();
        }



        public async Task<string> UploadFile(Stream fileStream, string fileName, string bucket)
        {
            var path = $"{Guid.NewGuid()}_{fileName}";

            // Converting Stream to byte array
            // In Shaa Allah, hover over the code to see explanations
            using var memoryStram = new MemoryStream();
            await fileStream.CopyToAsync(memoryStram);
            var bytes = memoryStram.ToArray();

            // In Shaa Allah, this will uploade the file to Supabase
            await _supabaseClient.Storage.From(bucket).Upload(bytes, path);

            // In Shaa Allah, getting the public URL of the uploaded file
            var publicUrl = _supabaseClient.Storage.From(bucket).GetPublicUrl(path);

            return publicUrl;
        }

    }
}