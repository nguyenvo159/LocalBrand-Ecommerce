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
    private readonly IMapper _mapper;
    IOptions<SmtpSettings> _smtpSettings;

    public OrderService(IRepository<Order> orderRepository,
    IRepository<OrderItem> orderItemRepository,
    IRepository<Cart> cartRepository,
    IRepository<Discount> discountRepository,
    ICartService cartService,
    IMapper mapper,
    IOptions<SmtpSettings> smtpSettings)
    {
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _cartRepository = cartRepository;
        _discountRepository = discountRepository;
        _cartService = cartService;
        _mapper = mapper;
        _smtpSettings = smtpSettings;
    }

    public async Task<List<OrderDto>> GetAll()
    {
        var orders = await _orderRepository.GetAllOrderAsync(o => true);
        if (orders == null)
        {
            throw new ApplicationException("Order not found");
        }
        return _mapper.Map<List<OrderDto>>(orders);
    }

    public async Task<List<OrderDto>> GetAllByUserId(Guid userId)
    {
        var orders = await _orderRepository.GetAllOrderAsync(o => o.UserId == userId);
        if (orders == null)
        {
            throw new ApplicationException("Order not found");
        }
        return _mapper.Map<List<OrderDto>>(orders);
    }

    public async Task<OrderDto> GetById(Guid id)
    {
        var order = await _orderRepository.GetOrderAsync(o => o.Id == id);
        if (order == null)
        {
            throw new ApplicationException("Order not found");
        }
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> Create(OrderCreateDto orderCreateDto)
    {
        var cart = await _cartRepository.GetCartAsync(u => u.UserId == orderCreateDto.UserId);
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
        await SendOrderConfirmationEmail(order.Id);
        return _mapper.Map<OrderDto>(order);

    }

    public async Task<OrderDto> Update(OrderUpdateDto orderUpdateDto)
    {
        var order = await _cartRepository.GetOrderAsync(o => o.Id == orderUpdateDto.Id);
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
        var order = await _orderRepository.GetOrderAsync(o => o.Id == id);
        if (order == null)
        {
            throw new ApplicationException("Order not found");
        }
        await _orderRepository.DeleteAsync(id);
    }

    private async Task SendOrderConfirmationEmail(Guid orderId)
    {
        try
        {
            var order = await _orderRepository.GetOrderAsync(o => o.Id == orderId);
            var orderItem = _mapper.Map<OrderDto>(order).OrderItems;
            if (order == null)
            {
                throw new ApplicationException("Order not found to mail");
            }
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpSettings.Value.SenderName, _smtpSettings.Value.SenderEmail));
            message.To.Add(new MailboxAddress(order.UserName, order.UserEmail));
            message.Subject = "Order Confirmation";

            var builder = new BodyBuilder
            {
                HtmlBody = $@"
                <html>
                <body>
                    <h2>Order Confirmation</h2>
                    <p>Dear {order.UserName},</p>
                    <p>Thank you for your order! Here are the details:</p>
                    <table style='border-collapse: collapse; width: 100%;'>
                        <tr>
                            <th style='border: 1px solid #dddddd; padding: 8px;'>Order ID</th>
                            <td style='border: 1px solid #dddddd; padding: 8px;'>{(order.Id).ToString().ToUpper()}</td>
                        </tr>
                        <tr>
                            <th style='border: 1px solid #dddddd; padding: 8px;'>Order Status</th>
                            <td style='border: 1px solid #dddddd; padding: 8px;'>{order.OrderStatus}</td>
                        </tr>
                        <tr>
                            <th style='border: 1px solid #dddddd; padding: 8px;'>Phone Number</th>
                            <td style='border: 1px solid #dddddd; padding: 8px;'>{order.UserPhone}</td>
                        </tr>
                        <tr>
                            <th style='border: 1px solid #dddddd; padding: 8px;'>Delivery Address</th>
                            <td style='border: 1px solid #dddddd; padding: 8px;'>{order.Address}</td>
                        </tr>
                        
                    </table>

                    <h3>Order Items</h3>
                    <table style='border-collapse: collapse; width: 100%;'>
                        <tr>
                            <th style='border: 1px solid #dddddd; padding: 8px;'>Item Name</th>
                            <th style='border: 1px solid #dddddd; padding: 8px;'>Quantity</th>
                            <th style='border: 1px solid #dddddd; padding: 8px;'>Price</th>
                            <th style='border: 1px solid #dddddd; padding: 8px;'>Total</th>
                        </tr>
                        {string.Join("\n", orderItem.Select(item =>
                            $@"
                        <tr>
                            <td style='border: 1px solid #dddddd; padding: 8px; text-align: center;'>{item.ProductName}</td>
                            <td style='border: 1px solid #dddddd; padding: 8px; text-align: center;'>{item.Quantity}</td>
                            <td style='border: 1px solid #dddddd; padding: 8px; text-align: center;'>{item.ProductPrice:C0}</td>
                            <td style='border: 1px solid #dddddd; padding: 8px; text-align: center;'>{(item.ProductPrice * item.Quantity):C0}</td>
                        </tr>"))}
                        <tr>
                            <td style='border: 1px solid #dddddd; padding: 8px; text-align: end;'><b>Total Amount:</b> {order.TotalAmount:C0}</td>
                        </tr>
                    </table>

                    <p>If you have any questions or need assistance, please contact us.</p>
                    <p>Best regards,</p>
                    <p>Your Store Team</p>
                </body>
                </html>"
            };


            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(_smtpSettings.Value.Server, _smtpSettings.Value.Port, false);
                client.Authenticate(_smtpSettings.Value.Username, _smtpSettings.Value.Password);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
        catch (ApplicationException ex)
        {
            throw new ApplicationException("Error sending email", ex);
        }
    }
}
