using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Phonebook.Report.Domain.Entities;
using Phonebook.Report.Domain.Enums;
using Phonebook.Report.Infrastructure.Interfaces;

namespace Phonebook.Report.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IMongoCollection<Reports> _reportCollection;
        private readonly IMapper _mapper;
        public ReportRepository(IConfiguration configuration, IMapper mapper)
        {
            _mapper = mapper;
            var client = new MongoClient(configuration["DatabaseSettings:ConnectionString"]);
            var db = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);
            _reportCollection = db.GetCollection<Reports>(configuration["DatabaseSettings:ReportCollectionName"]);
        }

        public async Task<Reports> CreateReportAsync()
        {
            var report = new Reports
            {
                Status = ReportStatus.Preparing,
                CreatedDate = DateTime.Now
            };

            var newReports = _mapper.Map<Reports>(report);
            await _reportCollection.InsertOneAsync(newReports);
            return newReports;
        }

        public async Task<IList<Reports>> GetAllReportsAsync()
        {
            return await _reportCollection.Find(x => true).ToListAsync();
        }

        public async Task<Reports> GetReportByIdAsync(string id)
        {
            return await _reportCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task ReportCompletedAsync(string id)
        {
            var filter = Builders<Reports>.Filter.Eq(s => s.Id, id);
            var update = Builders<Reports>.Update
                .Set(s => s.Status, ReportStatus.Completed)
                .Set(s => s.CompletedDate, DateTime.UtcNow);

            await _reportCollection.UpdateOneAsync(filter, update);
        }
    }
}
