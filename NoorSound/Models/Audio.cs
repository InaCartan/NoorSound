// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\

using Postgrest.Attributes;
using Postgrest.Models;

namespace NoorSound.Models
{
    [Table("Audio")]
    public class Audio : BaseModel
    {
        
        [PrimaryKey("id", false)]
        public int Id { get; set; }
        

        [Column("audio_name")]
        public string AudioName { get; set; }


        [Column("image_url")]
        public string ImageUrl { get; set; }


        [Column("audio_url")]
        public string AudioUrl { get; set; }

        // Foreign key to Admin
        [Reference(typeof(Admin), includeInQuery: true)]
        public Admin Admin { get; set; }


        [Column("made_at", ignoreOnInsert: true)]
        public DateTime MadeAt { get; set; }
    }
}
