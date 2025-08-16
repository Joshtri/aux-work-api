using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuxWork.Api.Models
{
    [Index(nameof(WorkItemId), nameof(Order), IsUnique = true)]
    public class ChecklistItem
    {
        public Guid Id { get; set; }
        public Guid WorkItemId { get; set; }
        public WorkItem? WorkItem { get; set; }

        [MaxLength(280)] public string Text { get; set; } = default!;
        public bool IsDone { get; set; }
        public int Order { get; set; } = 0;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
