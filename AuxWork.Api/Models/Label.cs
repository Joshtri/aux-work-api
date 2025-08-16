using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuxWork.Api.Models
{
    [Index(nameof(ProjectId), nameof(Name), IsUnique = true)]
    public class Label
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }

        [MaxLength(64)] public string Name { get; set; } = default!;
        [MaxLength(32)] public string? Color { get; set; } // e.g. #FF7A00

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        // Nav
        public List<WorkItemLabel> WorkItemLabels { get; set; } = new();
    }
}
