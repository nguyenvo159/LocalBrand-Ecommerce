using System;
using backend.Dtos.Contact;

namespace backend.Services;

public interface IEmailService
{
    Task SendOrderConfirmationEmail(Guid orderId);
    Task SendEmailContact(ContactCreateDto contactCreateDto);

}
