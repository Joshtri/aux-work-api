namespace AuxWork.Api.Common;

public record ApiResponse<T>(string status, string? message, T? data);

public record PagingMeta(int page, int pageSize, int total, int totalPages, bool hasNext, bool hasPrev);

public record PagedResponse<T>(string status, string? message, IReadOnlyList<T> data, PagingMeta paging);

public static class ApiResponses
{
    public static ApiResponse<T> Success<T>(T data, string? message = null)
        => new("success", message, data);

    public static ApiResponse<T> Fail<T>(string message, T? data = default)
        => new("fail", message, data);
}

public static class PagedResponses
{
    public static PagedResponse<T> Success<T>(IReadOnlyList<T> items, int page, int pageSize, int total, string? message = null)
    {
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);
        return new("success", message, items, new(page, pageSize, total, totalPages, page < totalPages, page > 1));
    }
}
