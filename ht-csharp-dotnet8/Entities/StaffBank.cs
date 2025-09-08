using System.ComponentModel.DataAnnotations.Schema;

namespace ht_csharp_dotnet8.Entities
{
    public class StaffBank : BaseEntity
    {
        [Column(Order = 1)]
        [ForeignKey("Staff")]
        public Guid StaffId { get; set; }

        [Column(Order = 2)]
        public string? BankName { get; set; }

        [Column(Order = 3)]
        public string? BankAccName { get; set; }

        [Column(Order = 4)]
        public string? BankAccNo { get; set; }
    }
}
