using AuxWork.Api.Models;

namespace AuxWork.Api.Repositories.WorkItems;

public record WorkItemQuery(
    Guid? ProjectId = null,
    WorkType? Type = null,
    WorkStatus? Status = null,
    Guid? AssigneeId = null,
    string? Label = null,
    string? Q = null,
    DateTime? DueFrom = null,
    DateTime? DueTo = null,
    string? Sort = "-priority,createdAt",
    int Page = 1,
    int PageSize = 20);
