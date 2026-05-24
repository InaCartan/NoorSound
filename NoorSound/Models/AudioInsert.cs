// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\

using Postgrest.Attributes;
using Postgrest.Models;

namespace NoorSound.Models
{
    [Table("audio")]
    public class AudioInsert : BaseModel
    {
        [Column("audio_name")]
        public string AudioName { get; set; }

        [Column("image_url")]
        public string ImageUrl { get; set; }

        [Column("audio_url")]
        public string AudioUrl { get; set; }

        [Column("admin_id")]
        public int AdminId { get; set; }

    }
}
