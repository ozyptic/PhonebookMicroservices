using Phonebook.Contact.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Contact.Infrastracture.Interfaces
{
    public interface IContactsRepository
    {
        Task<Contacts> AddContactAsync(Contacts contacts);
        Task<bool> DeleteContactAsync(string id);
        Task<Contacts> GetContactByIdAsync(string contactId);
        Task<List<Contacts>> GetAllContactsAsListAsync();

    }
}
