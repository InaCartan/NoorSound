using NoorSound.Models;

namespace NoorSound.Services.Interfaces
{
    public interface IDataService
    {   
        // -- Audios --
        Task AddAudio(AudioInsert audio);
        Task <IEnumerable<Audio>> GetAudios();
        Task UpdateAudio(Audio audio);
        Task DeleteAudio(long id);


        // -- Playlists --
        Task AddPlaylist(PlaylistInsert playlist);
        Task <IEnumerable<Playlist>> GetPlaylists();

        Task AddAudioToPlaylist(PlaylistAudioInsert playlistAudio);
        Task<IEnumerable<PlaylistAudio>> GetPlaylistAudios();


        // -- Admin --
        Task<Admin?> GetAdmin(string id);


        // -- Storage --
        Task DeleteFileFromStorage(string bucket, string path);
        Task<(string Path, string PublicUrl)> UploadFile(Stream fileStream, string fileName, string bucket);

    }
}
