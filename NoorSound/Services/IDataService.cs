using NoorSound.Models;

namespace NoorSound.Services
{
    public interface IDataService
    {   
        Task AddAudio(AudioInsert audio);
        Task <IEnumerable<Audio>> GetAudios();
        Task UpdateAudio(Audio audio);
        Task DeleteAudio(long id);

        Task DeleteFileFromStorage(string bucket, string path);
        Task<(string Path, string PublicUrl)> UploadFile(Stream fileStream, string fileName, string bucket);

    }
}
