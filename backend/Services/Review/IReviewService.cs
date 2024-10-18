using backend.Dto.Common;
using backend.Dto.Review;

namespace backend;

public interface IReviewService
{
    Task<ReviewDto?> GetById(Guid id);
    Task<ReviewDto?> GetByUserId(Guid userId);
    Task<List<ReviewDto>> GetByProductId(Guid productId);
    Task<PageResult<ReviewDto>> GetPaging(PageRequest request);
    Task<ReviewDto> Create(ReviewCreateDto reviewCreateDto);
    Task<ReviewDto> Update(ReviewUpdateDto reviewUpdateDto);

    Task Delete(Guid id);

}
