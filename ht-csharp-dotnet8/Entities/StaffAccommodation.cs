using System.ComponentModel.DataAnnotations.Schema;

namespace ht_csharp_dotnet8.Entities
{
    public class StaffAccommodation : BaseEntity
    {
        [Column(Order = 1)]
        [ForeignKey("Staff")]
        public Guid StaffId { get; set; }

        [Column(Order = 2)]
        public string? HostelName { get; set; }

        [Column(Order = 3)]
        public string? HostelRoom { get; set; }
    }
}
