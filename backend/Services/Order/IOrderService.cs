using backend.Dto.Order;

namespace backend.Services;

public interface IOrderService
{
    Task<List<OrderDto>> GetAll();
    Task<List<OrderDto>> GetAllByUserId(Guid userId);
    Task<OrderDto> GetById(Guid id);
    Task<OrderDto> Create(OrderCreateDto orderCreateDto);
    Task<OrderDto> Update(OrderUpdateDto orderUpdateDto);
    Task Delete(Guid id);

}
