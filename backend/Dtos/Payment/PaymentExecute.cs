using System;

namespace backend.Dtos.Payment;

public class PaymentExecute
{
    public string OrderId { get; set; }
    public string Amount { get; set; }
    public string OrderInfo { get; set; }
}
