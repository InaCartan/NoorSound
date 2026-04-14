using NoorSound.Models;

namespace NoorSound.Services
{
    public interface IDataService
    {
        
        Task AddAudio(Audio audio);
        Task <IEnumerable<Audio>> GetAudios();
        Task UpdateAudio(Audio audio);
        Task DeleteAudio(int id);

        Task<string> UploadFile(Stream fileStream, string fileName, string bucket);

    }
}
