using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using backend.Dto.Common;
using backend.Dtos.Contact;
using backend.Entities;
using backend.Repositories;

namespace backend.Services
{
    public class ContactService : IContactService
    {
        IEmailService _emailService;
        private readonly IRepository<Contact> _rpContact;
        private readonly IMapper _mapper;
        public ContactService(IEmailService emailService, IRepository<Contact> rpContact, IMapper mapper)
        {
            _emailService = emailService;
            _rpContact = rpContact;
            _mapper = mapper;
        }

        public async Task<Guid> Create(ContactCreateDto contact)
        {
            var data = _mapper.Map<Contact>(contact);
            var result = await _rpContact.AddAsync(data);
            await _emailService.SendEmailContact(contact);
            return result.Id;
        }

        public async Task Delete(Guid id)
        {
            var contact = await _rpContact.FindAsync(x => x.Id == id);
            if (contact == null)
            {
                throw new Exception("Contact not found");
            }
            await _rpContact.DeleteAsync(contact.Id);
        }

        public Task<PageResult<ContactDto>> GetPaging(PageRequest request)
        {
            var query = _rpContact.AsQueryable();
            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x => x.Name.Contains(request.Search) || x.Email.Contains(request.Search));
            }
            var totalRecords = query.Count();
            if (request.PageSize.HasValue && request.PageNumber.HasValue)
            {
                query = query.Skip((request.PageNumber.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value);
            }
            var data = query.ToList();
            data = data.OrderByDescending(x => x.CreatedAt).ToList();
            var result = new PageResult<ContactDto>
            {
                TotalRecords = totalRecords,
                Items = _mapper.Map<List<ContactDto>>(data),
                PageSize = request.PageSize ?? 1,
                PageNumber = request.PageNumber ?? 1
            };
            result.Items = result.Items.OrderByDescending(x => x.CreatedAt).ToList();
            return Task.FromResult(result);
        }

        public async Task<Guid> Update(ContactUpdateDto contact)
        {
            var data = await _rpContact.FindAsync(x => x.Id == contact.Id);
            if (data == null)
            {
                throw new Exception("Contact not found");
            }
            data = _mapper.Map(contact, data);
            await _rpContact.UpdateAsync(data);
            if (!string.IsNullOrEmpty(contact.Reply))
                await _emailService.SendEmailContactReply(data, contact.Reply);
            return data.Id;
        }
    }
}