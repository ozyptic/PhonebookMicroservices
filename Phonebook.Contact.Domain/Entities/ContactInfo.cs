using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phonebook.Contact.Domain.Enums;

namespace Phonebook.Contact.Domain.Entities
{
    public class ContactInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public required string ContactId { get; set; }
        public required string Value { get; set; }
        public ContactInfoType ContactInfoType { get; set; }
    }
}
