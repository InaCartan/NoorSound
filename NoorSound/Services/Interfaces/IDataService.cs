using NoorSound.Models;

namespace NoorSound.Services.Interfaces
{
    public interface IDataService
    {   
        // -- Audios --
        Task<long> AddAudio(AudioInsert audio);
        Task <IEnumerable<Audio>> GetAudios();
        Task UpdateAudio(Audio audio);
        Task DeleteAudio(long id);


        // -- Playlists --
        Task<long> AddPlaylist(PlaylistInsert playlist);
        Task <IEnumerable<Playlist>> GetPlaylists();
        Task DeletePlaylist(long id);


        // -- Playlists (junction table) --
        Task AddAudiosToPlaylist(IEnumerable<PlaylistAudioInsert> playlistAudio);
        Task<IEnumerable<PlaylistAudio>> GetPlaylistAudios();


        // -- Admin --
        Task<Admin?> GetAdmin(string id);
        Task<IEnumerable<Audio>> GetAudiosByAdmin(string adminid);
        


        // -- Storage --
        Task DeleteFileFromStorage(string bucket, string path);
        Task<(string Path, string PublicUrl)> UploadFile(Stream fileStream, string fileName, string bucket);

    }
}
