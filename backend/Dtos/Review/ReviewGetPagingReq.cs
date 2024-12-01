
using backend.Dto.Common;

public class ReviewGetPagingReq : PageRequest
{
    public int? Filter { get; set; }
}