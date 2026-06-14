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
        public string AudioName { get; set; } = string.Empty;


        [Column("image_url")]
        public string ImageUrl { get; set; } = string.Empty;


        [Column("audio_url")]
        public string AudioUrl { get; set; } = string.Empty;


        [Column("admin_id")]
        public string AdminId { get; set; } = string.Empty;


        [Reference(typeof(Admin), includeInQuery: true)] // Foreign key to Admin
        public Admin? Admin { get; set; } 



        [Column("made_at", ignoreOnInsert: true)]
        public DateTime MadeAt { get; set; }
    }
}
