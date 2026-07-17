// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\

using Postgrest.Attributes;
using Postgrest.Models;

namespace NoorSound.Models
{
    [Table("audio")]
    public class AudioInsert : BaseModel
    {
        [Column("audio_name")]
        public string AudioName { get; set; } = string.Empty;



        [Column("image_url")]
        public string ImageUrl { get; set; } = string.Empty;

        [Column("image_path")]
        public string ImagePath { get; set; } = string.Empty;



        [Column("audio_url")]
        public string AudioUrl { get; set; } = string.Empty;

        [Column("audio_path")]
        public string AudioPath { get; set; } = string.Empty;



        [Column("admin_id")]
        public string AdminId { get; set; } = string.Empty;

    }
}
