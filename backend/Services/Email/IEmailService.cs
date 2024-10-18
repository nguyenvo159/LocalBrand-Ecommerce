using System;
using backend.Dtos.Contact;
using backend.Entities;
using backend.Entity;

namespace backend.Services;

public interface IEmailService
{
    Task SendOrderConfirmationEmail(Guid orderId);
    Task SendEmailContact(ContactCreateDto contactCreateDto);
    Task SendEmailContactReply(Contact contact, string reply);
    Task SendEmailDiscount(Discount discount, string email);

}
