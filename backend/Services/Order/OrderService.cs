using AutoMapper;
using backend.Dto.Cart;
using backend.Dto.Order;
using backend.Entity;
using backend.Extensions;
using backend.Repositories;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace backend.Services;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<OrderItem> _orderItemRepository;
    private readonly IRepository<Cart> _cartRepository;
    private readonly IRepository<Discount> _discountRepository;
    private readonly ICartService _cartService;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;

    public OrderService(IRepository<Order> orderRepository,
    IRepository<OrderItem> orderItemRepository,
    IRepository<Cart> cartRepository,
    IRepository<Discount> discountRepository,
    ICartService cartService,
    IEmailService emailService,
    IMapper mapper)
    {
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _cartRepository = cartRepository;
        _discountRepository = discountRepository;
        _cartService = cartService;
        _emailService = emailService;
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
        order.OrderStatus = "Pending";

        //Check code
        if (!string.IsNullOrEmpty(orderCreateDto.Code))
        {
            var discount = await _discountRepository.FindAsync(d => d.Code == orderCreateDto.Code);
            if (discount != null && discount.ExpiryDate >= DateTime.Now && discount.RequireMoney <= total)
            {
                var discountAmount = total * discount.DiscountPercentage / 100;
                if (discountAmount > discount.MaximumDiscount)
                {
                    discountAmount = discount.MaximumDiscount;
                }
                total -= discountAmount;
                await _discountRepository.DeleteAsync(discount.Id);
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
        var order = await _orderRepository.FindAsync(o => o.Id == orderUpdateDto.Id);
        if (order == null)
        {
            throw new ApplicationException("Order not found");
        }

        order = _mapper.Map(orderUpdateDto, order);
        await _orderRepository.UpdateAsync(order);
        return _mapper.Map<OrderDto>(order);
    }

    public async Task Delete(Guid id)
    {
        var order = await _orderRepository.FindAsync(o => o.Id == id);
        if (order == null)
        {
            throw new ApplicationException("Order not found");
        }
        await _orderRepository.DeleteAsync(id);
    }


}
