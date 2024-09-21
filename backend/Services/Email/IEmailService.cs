using System;

namespace backend.Services;

public interface IEmailService
{
    Task SendOrderConfirmationEmail(Guid orderId);

}
