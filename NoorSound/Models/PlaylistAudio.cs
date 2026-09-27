using Postgrest.Attributes;
using Postgrest.Models;

namespace NoorSound.Models
{
    [Table("playlist_audio")]
    public class PlaylistAudio : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }


        [Column("made_at", ignoreOnInsert: true)]
        public DateTime MadeAt { get; set; }


        // ** Foreign keys & ID's**

        [Column("playlist_id")]
        public string PlaylistId { get; set; } = string.Empty; // ID

        [Reference(typeof(Playlist), includeInQuery: true)] // Foreign key to Playlist
        public Playlist? Playlist { get; set; }


        [Column("audio_id")]
        public long audioId { get; set; } // ID
        
        [Reference(typeof(Audio), includeInQuery: true)] // Foreign key to Audio
        public Audio? Audio { get; set; }





    }
}
