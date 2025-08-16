using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace AuxWork.Api.Infrastructure.Routing;

public sealed class SnakeCaseParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value is null) return null;
        var s = value.ToString()!;
        // "WorkItems" -> "work_items", "ProjectMembers" -> "project_members"
        return Regex.Replace(s, "([a-z0-9])([A-Z])", "$1_$2").ToLowerInvariant();
    }
}
