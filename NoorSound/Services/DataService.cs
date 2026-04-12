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

    }
}