// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\
using Postgrest.Attributes;
using Postgrest.Models;

namespace NoorSound.Models
{
    [Table("admin")]
    public class AdminInsert : BaseModel
    {

        [Column("id")] 
        public string Id { get; set; }

        [Column("admin_name")]
        public string Name { get; set; }

        [Column("admin_image_url")]
        public string AdminImageUrl { get; set; }

    }
}
