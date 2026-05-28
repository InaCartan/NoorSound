// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\

using Postgrest.Attributes;
using Postgrest.Models;


namespace NoorSound.Models
{
    [Table("audio")]
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


        [Column("admin_id")]
        public int AdminId { get; set; }


        [Reference(typeof(Admin), includeInQuery: true)] // Foreign key to Admin
        public Admin Admin { get; set; } 



        [Column("made_at", ignoreOnInsert: true)]
        public DateTime MadeAt { get; set; }
    }
}
