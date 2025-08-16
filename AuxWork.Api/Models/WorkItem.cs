using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AuxWork.Api.Models;

[Index(nameof(ProjectId), nameof(Status), nameof(Priority))]
public class WorkItem
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }

    // Subtask (opsional)
    public Guid? ParentId { get; set; }
    public WorkItem? Parent { get; set; }
    public List<WorkItem> Children { get; set; } = new();

    public WorkType Type { get; set; } = WorkType.Task;

    [MaxLength(200)] public string Title { get; set; } = default!;
    public string? Description { get; set; }

    public WorkStatus Status { get; set; } = WorkStatus.Backlog;
    public Priority Priority { get; set; } = Priority.Medium;

    public int? StoryPoints { get; set; }
    public double? EstimateHours { get; set; }
    public DateTime? DueDate { get; set; }

    public Guid? AssigneeId { get; set; }
    public User? Assignee { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    // Nav
    public List<WorkItemLabel> WorkItemLabels { get; set; } = new();
    public List<ChecklistItem> Checklist { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<TimeLog> TimeLogs { get; set; } = new();
}
