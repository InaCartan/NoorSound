// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\
using Postgrest.Attributes;
using Postgrest.Models;

namespace NoorSound.Models
{

    // ** Seems that this class isn't used in this project **
    [Table("admin")]
    public class AdminInsert : BaseModel
    {

        [Column("id")] 
        public string Id { get; set; } = string.Empty;

        [Column("admin_name")]
        public string Name { get; set; } = string.Empty;

        [Column("admin_image_url")]
        public string? AdminImageUrl { get; set; } 

    }
}
