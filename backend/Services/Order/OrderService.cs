using AutoMapper;
using backend.Dto.Cart;
using backend.Dto.Order;
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
    IMapper mapper)
    {
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _cartRepository = cartRepository;
        _discountService = discountService;
        _cartService = cartService;
        _emailService = emailService;
        _discountRepository = discountRepository;
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

        order = _mapper.Map(orderUpdateDto, order);
        await _orderRepository.UpdateAsync(order);
        order.UpdatedAt = DateTime.UtcNow;
        return _mapper.Map<OrderDto>(order);
    }

    public async Task Delete(Guid id)
    {
        // Load order và bao gồm OrderItems
        var order = await _orderRepository
            .FindAsync(o => o.Id == id && o.IsActived);
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

        // Cập nhật order
        await _orderRepository.UpdateAsync(order);

    }



}
