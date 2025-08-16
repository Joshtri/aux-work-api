using System.ComponentModel.DataAnnotations;

namespace AuxWork.Api.DTOs;

public record CreateProjectDto(
    [Required, StringLength(200)] string Name,
    [StringLength(40)] string? Code);

public record UpdateProjectDto(
    [Required, StringLength(200)] string Name,
    [StringLength(40)] string? Code,
    bool IsArchived);

public record ProjectDto(
    Guid Id,
    string Name,
    string? Code,
    bool IsArchived,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
