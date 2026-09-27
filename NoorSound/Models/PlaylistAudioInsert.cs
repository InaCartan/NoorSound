using Postgrest.Attributes;
using Postgrest.Models;

namespace NoorSound.Models
{
    [Table("playlist_audio")]
    public class PlaylistAudioInsert : BaseModel
    {
        // ** Foreign keys & ID's**

        [Column("playlist_id")]
        public string PlaylistId { get; set; } = string.Empty; // ID

        [Reference(typeof(Playlist), includeInQuery: true)] // Foreign key to Playlist
        public Playlist? Playlist { get; set; }


        [Column("audio_id")]
        public long AudioId { get; set; } // ID

        [Reference(typeof(Audio), includeInQuery: true)] // Foreign key to Audio
        public Audio? Audio { get; set; }

    }
}
