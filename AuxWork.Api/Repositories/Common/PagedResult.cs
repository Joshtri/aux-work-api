﻿namespace AuxWork.Api.Repositories.Common;

public record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int Total);
