using System;

namespace backend.Dtos.Payment;

public class PaymentCreateRequest
{
    public string FullName { get; set; }
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public string OrderInfo { get; set; }
}
