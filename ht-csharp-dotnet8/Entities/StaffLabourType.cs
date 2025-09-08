using System.ComponentModel.DataAnnotations.Schema;

namespace ht_csharp_dotnet8.Entities
{
    public class StaffLabourType : BaseEntity
    {
        [Column(Order = 1)]
        [ForeignKey("Staff")]
        public Guid StaffId { get; set; }

        [Column(Order = 2)]
        public bool? IsRate { get; set; }

        [Column(Order = 3)]
        public decimal? FootRate { get; set; }

        [Column(Order = 4)]
        public decimal? BodyRate { get; set; }

        [Column(Order = 5)]
        public bool? IsGuaranteeIncome { get; set; }

        [Column(Order = 6)]
        public decimal? GuaranteeIncomeAmount { get; set; }

        [Column(Order = 7)]
        public bool? IsPercentage { get; set; }

        [Column(Order = 8)]
        public decimal? PercentageRate { get; set; }
    }
}
