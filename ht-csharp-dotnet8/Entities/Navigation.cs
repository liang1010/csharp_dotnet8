using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ht_csharp_dotnet8.Entities
{
    [Index(nameof(Label), IsUnique = true)]
    public class Navigation : BaseEntity
    {
        [Column(Order = 1)]
        public string Label { get; set; }

        [Column(Order = 2)]
        public string Icon { get; set; }

        [Column(Order = 3)]
        public string Route { get; set; }

        // 🔁 Self-referencing: One navigation can have many children
        [Column(Order = 4)]
        public List<Navigation> Children { get; set; } = new();

        // 🔁 Foreign key to parent navigation (nullable for root items)
        [Column(Order = 5)]
        [ForeignKey("Parent")]
        public Guid? ParentId { get; set; }
        [JsonIgnore]
        public virtual Navigation? Parent { get; set; }

    }
}
