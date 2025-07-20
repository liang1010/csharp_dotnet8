using System.ComponentModel.DataAnnotations;

namespace ht_csharp_dotnet8.Entities
{
    public class Log
    {
        [Key]
        public int Id { get; set; }

        public string? Message { get; set; }

        [Required]
        [MaxLength(128)]
        public string Level { get; set; } = null!;

        [Required]
        public DateTime Timestamp { get; set; }

        public string? Exception { get; set; }

        [MaxLength(256)]
        public string? ActionName { get; set; }

        [MaxLength(128)]
        public string? MachineName { get; set; }

        [MaxLength(128)]
        public string? Application { get; set; }

        [MaxLength(64)]
        public string? ClientIp { get; set; }
    }
}
