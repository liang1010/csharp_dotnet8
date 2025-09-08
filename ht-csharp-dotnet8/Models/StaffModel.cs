namespace ht_csharp_dotnet8.Models
{
    public class StaffModel
    {
        public Guid Id { get; set; }
        public string? NickName { get; set; }
        public string? FullName { get; set; }
        public string? HostelName { get; set; }
        public string? HostelRoom { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
        public string? BankName { get; set; }
        public string? BankAccName { get; set; }
        public string? BankAccNo { get; set; }
        public bool? IsRate { get; set; }
        public decimal? FootRate { get; set; }
        public decimal? BodyRate { get; set; }
        public bool? IsGuaranteeIncome { get; set; }
        public decimal? GuaranteeIncomeAmount { get; set; }
        public bool? IsPercentage { get; set; }
        public decimal? PercentageRate { get; set; }
    }
}
