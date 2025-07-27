using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ht_csharp_dotnet8.Entities
{
    public class NavigationRoles : BaseEntity
    {
        [ForeignKey("ApplicationRole")]
        public Guid RoleId { get; set; }
        [ForeignKey("Navigation")]
        public Guid NavigationId { get; set; }
    }
}
