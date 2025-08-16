using System.ComponentModel.DataAnnotations;
using AuxWork.Api.Models;

namespace AuxWork.Api.DTOs;

public record CreateWorkItemDto(
    [Required] Guid ProjectId,
    [Required] WorkType Type,
    [Required, StringLength(200)] string Title,
    string? Description,
    Priority? Priority,
    int? StoryPoints,
    double? EstimateHours,
    DateTime? DueDate,
    Guid? AssigneeId
);

public record UpdateWorkItemDto(
    WorkStatus Status,
    string? Title,
    string? Description,
    Priority? Priority,
    int? StoryPoints,
    double? EstimateHours,
    DateTime? DueDate,
    Guid? AssigneeId
);

public record TransitionDto([Required] WorkStatus To);

public record WorkItemDto(
    Guid Id, Guid ProjectId, WorkType Type, string Title, string? Description,
    WorkStatus Status, Priority Priority, int? StoryPoints, double? EstimateHours,
    DateTime? DueDate, Guid? AssigneeId, DateTime CreatedAtUtc, DateTime UpdatedAtUtc
);
        