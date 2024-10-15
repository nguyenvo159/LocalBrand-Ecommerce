using System;

namespace backend.Dto.Common;

public class PageRequest
{
    public int? PageNumber { get; set; } = 1;
    public int? PageSize { get; set; }
    public string? Search { get; set; }
}
