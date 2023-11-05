using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Phonebook.Contact.Domain.Dtos;
using Phonebook.Contact.Domain.Entities;
using Phonebook.Contact.Infrastracture.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Contact.Infrastracture.Repositories
{
    public class ContactsRepository : IContactsRepository
    {
        private readonly IMongoCollection<Contacts> _contactCll;
        private readonly IConfiguration _configuration;

        public ContactsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            var client = new MongoClient(_configuration["DatabaseSettings:ConnectionString"]);
            var db = client.GetDatabase(_configuration["DatabaseSettings:DatabaseName"]);
            _contactCll = db.GetCollection<Contacts>(_configuration["DatabaseSettings:ContactCollectionName"]);
        }
        public async Task<Contacts> AddContactAsync(Contacts contact)
        {
            await _contactCll.InsertOneAsync(contact);
            return contact;
        }

        public async Task<bool> DeleteContactAsync(string id)
        {
            var result = await _contactCll.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<List<Contacts>> GetAllContactsAsListAsync()
        {
            return await _contactCll.Find(c => true).ToListAsync();
        }

        public async Task<Contacts> GetContactByIdAsync(string contactId)
        {
            return await _contactCll.Find(c => true && c.Id == contactId).FirstOrDefaultAsync();
        }
    }
}
