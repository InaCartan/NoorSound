using NoorSound.Models;

namespace NoorSound.Services.Interfaces
{
    public interface IDataService
    {   
        Task AddAudio(AudioInsert audio);
        Task <IEnumerable<Audio>> GetAudios();
        Task<Admin?> GetAdmin(string id);
        Task UpdateAudio(Audio audio);
        Task DeleteAudio(long id);

        Task DeleteFileFromStorage(string bucket, string path);
        Task<(string Path, string PublicUrl)> UploadFile(Stream fileStream, string fileName, string bucket);

    }
}
