using backend.Entity;

namespace backend.Services;

public interface IDiscountService
{
    Task<List<Discount>> GetAll();
    Task<Discount> GetById(Guid id);
    Task<Discount> GetByCode(string code);
    Task<List<Discount>> Create(DiscountCreateDto discountCreateDto);
    Task Delete(Guid id);
    Task<byte[]> Export();
    Task<List<string>> DeleteDiscountExpired();

}
