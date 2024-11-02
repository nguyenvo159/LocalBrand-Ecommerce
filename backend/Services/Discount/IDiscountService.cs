using backend.Dto.Common;
using backend.Entity;
using CloudinaryDotNet.Actions;

namespace backend.Services;

public interface IDiscountService
{
    Task<List<Discount>> GetAll();
    Task<Discount> GetById(Guid id);
    Task<Discount> GetByCode(string code);
    Task<PageResult<Discount>> GetPaging(PageRequest request);
    Task<List<Discount>> Create(DiscountCreateDto discountCreateDto);
    Task Delete(Guid id);
    Task<byte[]> Export();
    Task<List<string>> DeleteDiscountExpired();
    Task SendDiscount(DiscountSendReq request);

}
