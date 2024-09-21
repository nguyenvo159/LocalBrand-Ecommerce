using System;
using AutoMapper;
using backend.Dto.Order;
using backend.Entity;
using backend.Extensions;
using backend.Repositories;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace backend.Services;
public class EmailService : IEmailService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IMapper _mapper;
    IOptions<SmtpSettings> _smtpSettings;

    public EmailService(IRepository<Order> orderRepository,
    IMapper mapper,
    IOptions<SmtpSettings> smtpSettings)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _smtpSettings = smtpSettings;
    }



    public async Task SendOrderConfirmationEmail(Guid orderId)
    {
        try
        {
            var order = await _orderRepository.FindAsync(o => o.Id == orderId);
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
