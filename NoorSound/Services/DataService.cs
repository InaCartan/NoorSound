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
        
        
        public async Task AddAudio(Audio audio)
        {
            await _supabaseClient.From<Audio>().Insert(audio);
        }

        public async Task<IEnumerable<Audio>> GetAudios()
        {
            var response = await _supabaseClient.From<Audio>().Get();
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
            await _supabaseClient.From<Audio>().Where(a => a.Id == id).Delete();
        }

        public async Task<string> UploadFile(Stream fileStream, string fileName, string bucket)
        {
            var path = $"{Guid.NewGuid()}_{fileName}";

            // Converting Stream to byte array
            // In Shaa Allah, hover the mouse on the code to see explanation
            using var memoryStram = new MemoryStream();
            await fileStream.CopyToAsync(memoryStram);
            var bytes = memoryStram.ToArray();

            // In Shaa Allah, uploading the file to Supabase
            await _supabaseClient.Storage.From(bucket).Upload(bytes, path);

            // In Shaa Allah, getting the public URL of the uploaded file
            var publicUrl = _supabaseClient.Storage.From(bucket).GetPublicUrl(path);

            return publicUrl;
        }

    }
}