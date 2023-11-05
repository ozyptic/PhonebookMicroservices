using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Phonebook.Report.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Report.Domain.Entities
{
    public class Reports
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public ReportStatus Status { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedDate { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? CompletedDate { get; set; }
        
    }
}
