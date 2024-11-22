using backend.Dto.Common;
using backend.Dto.Order;
using backend.Dtos.Order;

namespace backend.Services;

public interface IOrderService
{
    Task<List<OrderDto>> GetAll();
    Task<List<OrderDto>> GetAllByUserId(Guid userId);
    Task<OrderDto> GetById(Guid id);
    Task<PageResult<OrderDto>> GetPaging(OrderGetPagingRequestDto orderPagingDto);
    Task<OrderDto> Create(OrderCreateDto orderCreateDto);
    Task<OrderDto> Update(OrderUpdateDto orderUpdateDto);
    Task Delete(Guid id);

    Task<OrderAnalyticsDto> GetOrderAnalytics();
    Task<AnalyticsDto> GetAnalytics(int month, int year);
}
