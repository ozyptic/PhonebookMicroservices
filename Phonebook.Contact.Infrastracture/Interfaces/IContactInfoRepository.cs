using Phonebook.Contact.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Contact.Infrastracture.Interfaces
{
    public interface IContactInfoRepository
    {
        Task<List<ContactInfo>> GetAllContactInfosAsListAsync();
        Task<ContactInfo> GetContactInfoByContactIdAsync(string contactId);
        Task<ContactInfo> AddContactInfoAsync(ContactInfo contactInfo);
        Task<bool> DeleteContactInfoAsync(string id);
    }
}
