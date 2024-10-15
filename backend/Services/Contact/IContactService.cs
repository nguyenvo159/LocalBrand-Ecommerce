using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dto.Common;
using backend.Dtos.Contact;

namespace backend.Services
{
    public interface IContactService
    {
        Task<Guid> Create(ContactCreateDto contact);
        Task<PageResult<ContactDto>> GetPaging(PageRequest request);
        Task<Guid> Update(ContactUpdateDto contact);
        Task Delete(Guid id);

    }
}