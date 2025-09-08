using System.ComponentModel.DataAnnotations.Schema;

namespace ht_csharp_dotnet8.Entities
{
    public class StaffContact : BaseEntity
    {
        [Column(Order = 1)]
        [ForeignKey("Staff")]
        public Guid StaffId { get; set; }

        [Column(Order = 2)]
        public string? PhoneNo { get; set; }

        [Column(Order = 3)]
        public string? Email { get; set; }
    }
}
