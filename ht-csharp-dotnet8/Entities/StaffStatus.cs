using System.ComponentModel.DataAnnotations.Schema;

namespace ht_csharp_dotnet8.Entities
{
    public class StaffStatus : BaseEntity
    {
        [Column(Order = 1)]
        [ForeignKey("Staff")]
        public Guid StaffId { get; set; }

        [Column(Order = 2)]
        public DateTime? CheckIn { get; set; }

        [Column(Order = 3)]
        public DateTime? CheckOut { get; set; }
    }
}
