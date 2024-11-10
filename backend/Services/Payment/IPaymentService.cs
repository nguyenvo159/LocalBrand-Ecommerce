using System;
using backend.Dtos.Payment;

namespace backend.Services;

public interface IPaymentService
{
    Task<PaymentResponse> CreatePayment(PaymentCreateRequest request);
}
