// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\

using Postgrest.Attributes;
using Postgrest.Models;


namespace NoorSound.Models
{
    [Table("admin")]
    public class Admin : BaseModel
    {
     
        [PrimaryKey("id", false)]
        public string Id { get; set; } = string.Empty;

        [Column("admin_name")]
        public string Name { get; set; } = string.Empty;

        [Column("admin_image_url")]
        public string? AdminImageUrl { get; set; }


        [Column("made_at", ignoreOnInsert: true)]
        public DateTime MadeAt { get; set; }
        
    }
}
