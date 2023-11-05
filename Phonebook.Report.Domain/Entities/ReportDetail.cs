using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Report.Domain.Entities
{
    public class ReportDetail
    {
        public ReportDetail(string? reportId, string? location, int contactCount, int phoneNumberCount)
        {
            ReportId = reportId;
            Location = location;
            ContactCount = contactCount;
            PhoneNumberCount = phoneNumberCount;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ReportId { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string? Location { get; set; }
        [BsonRepresentation(BsonType.Int32)]
        public int PhoneNumberCount { get; set; }
        [BsonRepresentation(BsonType.Int32)]
        public int ContactCount { get; set; }
        
    }
}
