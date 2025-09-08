using System.ComponentModel.DataAnnotations.Schema;

namespace ht_csharp_dotnet8.Entities
{
    public class Staff : BaseEntity
    {
        [Column(Order = 1)]
        public string? NickName { get; set; }

        [Column(Order = 2)]
        public string? FullName { get; set; }

        [Column(Order = 3)]
        public string? Gender { get; set; }

        [Column(Order = 4)]
        public string? Nationality { get; set; }

        [Column(Order = 5)]
        public string? Remark { get; set; }

        [Column(Order = 6)]
        public bool? IsConsultant { get; set; }

        [Column(Order = 7)]
        public bool? IsTherapist { get; set; }
    }
}
