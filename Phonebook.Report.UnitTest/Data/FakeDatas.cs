using Phonebook.Report.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phonebook.Report.Domain.Enums;

namespace Phonebook.Report.UnitTest.Data
{
    public static class FakeDatas
    {
        public static string? FakeReportId = "95abc180ae6c8bae74e074988";
        public static Reports? GetReportById(string? fakeReportId)
        {
            return GetReportsFake().FirstOrDefault(x => x?.Id == fakeReportId);
        }
        public static IList<Reports?> GetReportsFake()
        {
            return new List<Reports?>
            {
                new Reports
                {
                    CompletedDate = DateTime.Now,
                    Status = ReportStatus.Completed,
                    Id = "95abc180ae6c8bae74e074988"
                },
                new Reports
                {
                    CompletedDate = DateTime.Now,
                    Status = ReportStatus.Preparing,
                    Id = "95abc180ae6c8bae74e074987"
                },
            };
        }
    }
}
