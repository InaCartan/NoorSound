// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\

using Postgrest.Attributes;
using Postgrest.Models;

namespace NoorSound.Models
{
    [Table("playlist")]
    public class Playlist : BaseModel
    {

        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("playlist_name")]
        public string PlaylistName { get; set; } = string.Empty;



        [Column("admin_id")]
        public string AdminId { get; set; } = string.Empty;

        [Reference(typeof(Admin), includeInQuery: true)] // Foreign key to Admin
        public Admin? Admin { get; set; }


        [Column("made_at", ignoreOnInsert: true)]
        public DateTime MadeAt { get; set; }

    }
}
