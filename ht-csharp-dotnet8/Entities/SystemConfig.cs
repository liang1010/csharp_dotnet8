using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ht_csharp_dotnet8.Entities
{
    [Index("Code", IsUnique = true)]
    public class SystemConfig : BaseEntity
    {
        [Column("Code", Order = 1)]
        public string Code { get; set; }
        [Column("Value", Order = 2)]
        public string Value { get; set; }
    }
}
