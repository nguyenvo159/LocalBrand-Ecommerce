using AutoMapper;
using backend.Dto.Cart;
using backend.Dto.Common;
using backend.Dto.Order;
using backend.Dtos.Order;
using backend.Entity;
using backend.Extensions;
using backend.Repositories;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;

namespace backend.Services;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<OrderItem> _orderItemRepository;
    private readonly IRepository<Cart> _cartRepository;
    private readonly IRepository<ProductInventory> _productInventoryRepository;
    private readonly IDiscountService _discountService;
    private readonly IRepository<Discount> _discountRepository;
    private readonly ICartService _cartService;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;

    public OrderService(IRepository<Order> orderRepository,
    IRepository<OrderItem> orderItemRepository,
    IRepository<Cart> cartRepository,
    IDiscountService discountService,
    ICartService cartService,
    IEmailService emailService,
    IRepository<Discount> discountRepository,
    IRepository<ProductInventory> productInventoryRepository,
    IMapper mapper)
    {
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _cartRepository = cartRepository;
        _discountService = discountService;
        _cartService = cartService;
        _emailService = emailService;
        _discountRepository = discountRepository;
        _productInventoryRepository = productInventoryRepository;
        _mapper = mapper;
    }

    public async Task<List<OrderDto>> GetAll()
    {
        var orders = await _orderRepository.FindAllAsync(o => true);
        if (orders == null)
        {
            throw new ApplicationException("Order not found");
        }
        return _mapper.Map<List<OrderDto>>(orders);
    }

    public async Task<List<OrderDto>> GetAllByUserId(Guid userId)
    {
        var orders = await _orderRepository.FindAllAsync(o => o.UserId == userId);
        if (orders == null)
        {
            throw new ApplicationException("Order not found");
        }
        orders = orders.OrderByDescending(o => o.CreatedAt).ToList();
        return _mapper.Map<List<OrderDto>>(orders);
    }

    public async Task<OrderDto> GetById(Guid id)
    {
        var order = await _orderRepository.FindAsync(o => o.Id == id);
        if (order == null)
        {
            throw new ApplicationException("Order not found");
        }
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> Create(OrderCreateDto orderCreateDto)
    {
        var cart = await _cartRepository.FindAsync(u => u.UserId == orderCreateDto.UserId);
        if (cart == null || cart.CartItems.Count == 0)
        {
            throw new ApplicationException("Cart not found");
        }
        var total = _mapper.Map<CartDto>(cart).Total;

        var order = _mapper.Map<Order>(orderCreateDto);
        order.Status = Text.Enums.Enums.OrderStatus.Pending;
        order.IsActived = true;
        var shipCost = new decimal[] { 0, 35000, 50000, 25000 };
        total += shipCost[(int)order.ShipType];
        //Check code
        if (!string.IsNullOrEmpty(orderCreateDto.Code))
        {
            var discount = await _discountService.GetByCode(orderCreateDto.Code);
            if (discount != null && discount.ExpiryDate >= DateTime.Now && discount.RequireMoney <= total)
            {
                var discountAmount = total * discount.DiscountPercentage / 100;
                if (discountAmount > discount.MaximumDiscount)
                {
                    discountAmount = discount.MaximumDiscount;
                }
                total -= discountAmount;
                order.DiscountId = discount.Id;
                await _discountService.Delete(discount.Id);
            }
        }

        order.TotalAmount = total;
        await _orderRepository.AddAsync(order);

        foreach (var item in cart.CartItems)
        {
            var orderItem = _mapper.Map<OrderItem>(item);
            orderItem.OrderId = order.Id;
            await _orderItemRepository.AddAsync(orderItem);
        }

        //Clear cart
        await _cartService.ClearCart(cart.Id);
        //Mail
        await _emailService.SendOrderConfirmationEmail(order.Id);
        return _mapper.Map<OrderDto>(order);

    }

    public async Task<OrderDto> Update(OrderUpdateDto orderUpdateDto)
    {
        var order = await _orderRepository.FindAsync(o => o.Id == orderUpdateDto.Id && o.IsActived);
        if (order == null)
        {
            throw new ApplicationException("Order not found");
        }
        var oldData = order;

        order = _mapper.Map(orderUpdateDto, order);
        if (oldData.PayType.HasValue)
        {
            order.PayType = oldData.PayType.Value;
        }
        order.UpdatedAt = DateTime.UtcNow;
        await _orderRepository.UpdateAsync(order);

        if (order.Status == Text.Enums.Enums.OrderStatus.Cancelled)
        {
            // Xử lý discount nếu có
            if (order.DiscountId != null)
            {
                var discount = await _discountRepository.FindAsync(d => d.Id == order.DiscountId);
                if (discount != null)
                {
                    discount.IsActived = true;
                    await _discountRepository.UpdateAsync(discount);
                }
            }
            foreach (var item in order.OrderItems)
            {

                var inventory = await _productInventoryRepository.FindAsync(i => i.ProductId == item.ProductId && i.SizeId == item.SizeId);
                if (inventory != null)
                {
                    inventory.Inventory += item.Quantity;
                    await _productInventoryRepository.UpdateAsync(inventory);
                }
            }
        }
        return _mapper.Map<OrderDto>(order);
    }

    public async Task Delete(Guid id)
    {
        var order = await _orderRepository.AsQueryable().Include(o => o.OrderItems)
            .Where(o => o.Id == id && o.IsActived).FirstOrDefaultAsync();
        if (order == null)
        {
            throw new ApplicationException("Order not found");
        }

        // Hủy đơn hàng
        order.IsActived = false;
        order.Status = Text.Enums.Enums.OrderStatus.Cancelled;
        order.UpdatedAt = DateTime.UtcNow;

        // Xử lý discount nếu có
        if (order.DiscountId != null)
        {
            var discount = await _discountRepository.FindAsync(d => d.Id == order.DiscountId);
            if (discount != null)
            {
                discount.IsActived = true;
                await _discountRepository.UpdateAsync(discount);
            }
        }
        if (order.OrderItems != null && order.OrderItems.Count > 0)
        {
            foreach (var item in order.OrderItems.ToList())
            {

                var inventory = await _productInventoryRepository.FindAsync(i => i.ProductId == item.ProductId && i.SizeId == item.SizeId);
                if (inventory != null)
                {
                    inventory.Inventory += item.Quantity;
                    await _productInventoryRepository.UpdateAsync(inventory);
                }
            }
        }

        // Cập nhật order
        await _orderRepository.UpdateAsync(order);

    }

    public Task<PageResult<OrderDto>> GetPaging(OrderGetPagingRequestDto orderPagingDto)
    {
        var orders = _orderRepository.AsQueryable();
        if (orderPagingDto.UserId.HasValue)
        {
            orders = orders.Where(o => o.UserId == orderPagingDto.UserId);
        }
        if (orderPagingDto.Status.HasValue)
        {
            orders = orders.Where(o => o.Status == orderPagingDto.Status);
        }
        if (!string.IsNullOrEmpty(orderPagingDto.Search))
        {
            var searchLower = orderPagingDto.Search.ToLower();
            orders = orders.Where(o => o.Id.ToString().ToLower().Contains(searchLower)
                                    || o.UserPhone.ToLower().Contains(searchLower)
                                    || o.UserEmail.ToLower().Contains(searchLower)
                                    || o.UserName.ToLower().Contains(searchLower));
        }
        if (orderPagingDto.FromDate.HasValue)
        {
            orders = orders.Where(o => o.CreatedAt >= ConvertToUtc(orderPagingDto.FromDate));
        }
        if (orderPagingDto.ToDate.HasValue)
        {
            orders = orders.Where(o => o.CreatedAt <= ConvertToUtc(orderPagingDto.ToDate));
        }
        var totalRecords = orders.Count();
        orders = orders.OrderByDescending(a => a.CreatedAt);
        if (orderPagingDto.PageNumber.HasValue)
        {
            orders = orders.Skip((orderPagingDto.PageNumber.Value - 1) * orderPagingDto.PageSize.Value);
        }
        if (orderPagingDto.PageSize.HasValue)
        {
            orders = orders.Take(orderPagingDto.PageSize.Value);
        }
        var result = new PageResult<OrderDto>
        {
            TotalRecords = totalRecords,
            Items = _mapper.Map<List<OrderDto>>(orders.ToList()),
            PageNumber = orderPagingDto.PageNumber ?? 1,
            PageSize = orderPagingDto.PageSize ?? 100
        };
        return Task.FromResult(result);
    }

    public DateTime? ConvertToUtc(DateTime? dateTime)
    {
        if (dateTime.HasValue && dateTime.Value.Kind == DateTimeKind.Unspecified)
        {
            return DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc);
        }
        return dateTime?.ToUniversalTime();
    }


    public async Task<OrderAnalyticsDto> GetOrderAnalytics()
    {
        var now = DateTime.UtcNow;  // Ensure we are using UTC
        var startOfThisWeek = StartOfWeek(now);
        var startOfLastWeek = StartOfWeek(now.AddDays(-7));
        var startOfThisMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);  // Use DateTimeKind.Utc
        var startOfLastMonth = startOfThisMonth.AddMonths(-1);

        var orders = _orderRepository.AsQueryable();
        var ordersThisWeek = await orders.Where(o => o.UpdatedAt >= startOfThisWeek
                                    && o.Status == Text.Enums.Enums.OrderStatus.Done).CountAsync();
        var ordersLastWeek = await orders.Where(o => o.UpdatedAt >= startOfLastWeek
                                    && o.UpdatedAt < startOfThisWeek
                                    && o.Status == Text.Enums.Enums.OrderStatus.Done).CountAsync();
        var revenueThisWeek = await orders.Where(o => o.UpdatedAt >= startOfThisWeek
                                    && o.Status == Text.Enums.Enums.OrderStatus.Done).SumAsync(o => o.TotalAmount);
        var revenueLastWeek = await orders.Where(o => o.UpdatedAt >= startOfLastWeek
                                    && o.UpdatedAt < startOfThisWeek
                                    && o.Status == Text.Enums.Enums.OrderStatus.Done).SumAsync(o => o.TotalAmount);
        var revenueMonth = await orders.Where(o => o.CreatedAt >= startOfThisMonth && o.Status == Text.Enums.Enums.OrderStatus.Done)
                                    .SumAsync(o => o.TotalAmount);
        var revenueLastMonth = await orders.Where(o => o.CreatedAt >= startOfLastMonth && o.CreatedAt < startOfThisMonth
                                    && o.Status == Text.Enums.Enums.OrderStatus.Done).SumAsync(o => o.TotalAmount);

        var orderCancelThisWeek = await orders.Where(o => o.UpdatedAt >= startOfThisWeek && o.Status == Text.Enums.Enums.OrderStatus.Cancelled).CountAsync();
        var orderCancelLastWeek = await orders.Where(o => o.UpdatedAt >= startOfLastWeek && o.UpdatedAt < startOfThisWeek && o.Status == Text.Enums.Enums.OrderStatus.Cancelled).CountAsync();

        var orderAnalytics = new OrderAnalyticsDto
        {
            OrderCount = ordersThisWeek,
            OrderCountLastWeek = ordersLastWeek,
            RevenuesWeek = revenueThisWeek,
            RevenuesLastWeek = revenueLastWeek,
            RevenueMonth = revenueMonth,
            RevenueLastMonth = revenueLastMonth,
            CanceledOrdersThisWeek = orderCancelThisWeek,
            CanceledOrdersLastWeek = orderCancelLastWeek
        };
        return orderAnalytics;
    }

    private DateTime StartOfWeek(DateTime dt)
    {
        var diff = (7 + (dt.DayOfWeek - DayOfWeek.Monday)) % 7;
        return dt.AddDays(-1 * diff).Date.ToUniversalTime();  // Ensure the DateTime is in UTC
    }

}
