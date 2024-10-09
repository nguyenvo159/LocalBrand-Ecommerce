using System;
using AutoMapper;
using backend.Dto.Order;
using backend.Entity;
using backend.Extensions;
using backend.Helper.EnumHelper;
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
            var total = orderItem.Sum(item => item.ProductPrice * item.Quantity);
            var shipCost = new decimal[] { 0, 35000, 50000, 25000 };
            if (order == null)
            {
                throw new ApplicationException("Order not found to mail");
            }
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpSettings.Value.SenderName, _smtpSettings.Value.SenderEmail));
            message.To.Add(new MailboxAddress(order.UserName, order.UserEmail));
            message.Subject = "Xác Nhận Đơn Hàng - AMIE Fashion";

            var builder = new BodyBuilder
            {
                HtmlBody = $@"
                <html>
                <body>
                    <h1>XÁC NHẬN ĐƠN HÀNG</h1>
                    <p>Dear {order.UserName},</p>
                    <p>Cám ơn bạn đã đặt hàng sản phẩm của chúng tôi, dưới đây là chi tiết hóa đơn:</p>
                    <br>
                    <table style='border-collapse: collapse; width: 100%;'>
                        <tr>
                            <th style='border: 1px solid #dddddd; padding: 8px; text-align: start;'>Mã đơn hàng</th>
                            <td style='border: 1px solid #dddddd; padding: 8px; text-align: start;'>{(order.Id).ToString().ToUpper()}</td>
                        </tr>
                        <tr>
                            <th style='border: 1px solid #dddddd; padding: 8px; text-align: start;'>Trạng thái</th>
                            <td style='border: 1px solid #dddddd; padding: 8px; text-align: start;'>{order.Status.GetDescription()}</td>
                        </tr>
                        <tr>
                            <th style='border: 1px solid #dddddd; padding: 8px; text-align: start;'>Số điện thoại</th>
                            <td style='border: 1px solid #dddddd; padding: 8px; text-align: start;'>{order.UserPhone}</td>
                        </tr>
                        <tr>
                            <th style='border: 1px solid #dddddd; padding: 8px; text-align: start;'>Địa chỉ</th>
                            <td style='border: 1px solid #dddddd; padding: 8px; text-align: start;'>{order.Address}</td>
                        </tr>
                        <tr>
                            <th style='border: 1px solid #dddddd; padding: 8px; text-align: start;'>Ghi chú</th>
                            <td style='border: 1px solid #dddddd; padding: 8px; text-align: start;'>{order.Note ?? "Không có ghi chú"}</td>
                        </tr>
                        
                    </table>

                    <h3>Danh sách sản phẩm</h3>
                    <table style='border-collapse: collapse; width: 100%;'>
                        <tr>
                            <th style='border: 1px solid #dddddd; padding: 8px;'>Tên sản phẩm</th>
                            <th style='border: 1px solid #dddddd; padding: 8px;'>Số lượng</th>
                            <th style='border: 1px solid #dddddd; padding: 8px;'>Giá</th>
                            <th style='border: 1px solid #dddddd; padding: 8px;'>Thành tiền</th>
                        </tr>
                        {string.Join("\n", orderItem.Select(item =>
                            $@"
                        <tr>
                            <td style='border: 1px solid #dddddd; padding: 8px; text-align: start;'>{item.ProductName}</td>
                            <td style='border: 1px solid #dddddd; padding: 8px; text-align: center;'>{item.Quantity}</td>
                            <td style='border: 1px solid #dddddd; padding: 8px; text-align: center;'>{item.ProductPrice:C0}</td>
                            <td style='border: 1px solid #dddddd; padding: 8px; text-align: center;'>{(item.ProductPrice * item.Quantity):C0}</td>
                        </tr>"))}
                        
                    </table>
                    <p><b>Phí vận chuyển({order.ShipType.GetDescription()}):</b> {shipCost[(int)order.ShipType]:C0}</p>
                    <p><b>Giảm giá:</b> {(total + shipCost[(int)order.ShipType] - order.TotalAmount):C0}</p>
                    <p><b>Tổng tiền:</b> {order.TotalAmount:C0}</p>
                    <p><b>Hình thức thanh toán: </b> Thanh toán khi nhận hàng</p>
                    
                    <br>
                    <p>Vui lòng chuẩn bị {order.TotalAmount:C0} trước khi nhận hàng.</p>
                    <p>Nếu bạn có bất kỳ câu hỏi hay cần sự giúp đỡ nào hãy liên hệ lại với chúng tôi.</p> 
                    <br>
                    <p>Best regards,</p>
                    <p>AMIE Fashion</p>
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
