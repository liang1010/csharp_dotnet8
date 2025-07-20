using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ht_csharp_dotnet8.Entities
{
    public class BaseEntity
    {
        [Key]
        [Column("Id", Order = 0)]
        public int Id { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public Status Status { get; set; }
    }

}
