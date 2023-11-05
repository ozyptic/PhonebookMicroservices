using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Phonebook.Report.Domain.Entities;
using Phonebook.Report.Infrastructure.Interfaces;

namespace Phonebook.Report.Infrastructure.Repositories
{
    public class ReportDetailRepository : IReportDetailRepository
    {
        private readonly IMongoCollection<ReportDetail> _reportDetailsCollection;

        public ReportDetailRepository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["DatabaseSettings:ConnectionString"]);
            var db = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);
            _reportDetailsCollection = db.GetCollection<ReportDetail>(configuration["DatabaseSettings:ReportDetailsCollectionName"]);
        }
        public async Task CreateReportDetailsAsync(IList<ReportDetail> addReportDetails)
        {
            await _reportDetailsCollection.InsertManyAsync(addReportDetails);
        }

        public async Task<IList<ReportDetail>> GetDetailsByReportIdAsync(string? reportId)
        {
            return await _reportDetailsCollection.Find(x => x.ReportId == reportId).ToListAsync();
        }
    }
}
