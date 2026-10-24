// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\

using Postgrest.Attributes;
using Postgrest.Models;

namespace NoorSound.Models
{
    [Table("playlist")]
    public class PlaylistInsert : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("playlist_name")]
        public string PlaylistName { get; set; } = string.Empty;


        [Column("admin_id")]
        public string AdminId { get; set; } = string.Empty;
    }
}
