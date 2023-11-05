using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Phonebook.Contact.Domain.Entities;
using Phonebook.Contact.Infrastracture.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Contact.Infrastracture.Repositories
{
    public class ContactInfoRepository : IContactInfoRepository
    {
        private readonly IMongoCollection<ContactInfo> _contactInfoCll;
        private readonly IConfiguration _configuration;
        public ContactInfoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            var client = new MongoClient(_configuration["DatabaseSettings:ConnectionString"]);
            var db = client.GetDatabase(_configuration["DatabaseSettings:DatabaseName"]);
            _contactInfoCll = db.GetCollection<ContactInfo>(_configuration["DatabaseSettings:ContactInfosCollectionName"]);
        }
        public async Task<ContactInfo> AddContactInfoAsync(ContactInfo contactInfo)
        {
            await _contactInfoCll.InsertOneAsync(contactInfo);
            return contactInfo;
        }

        public async Task<bool> DeleteContactInfoAsync(string id)
        {
            var result = await _contactInfoCll.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<List<ContactInfo>> GetAllContactInfosAsListAsync()
        {
            return await _contactInfoCll.Find(c => true).ToListAsync();
        }

        public async Task<ContactInfo> GetContactInfoByContactIdAsync(string contactId)
        {
            return await _contactInfoCll.Find(c => true && c.ContactId == contactId).FirstOrDefaultAsync();
        }
    }
}
