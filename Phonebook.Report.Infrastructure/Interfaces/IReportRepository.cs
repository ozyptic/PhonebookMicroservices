using Phonebook.Report.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Report.Infrastructure.Interfaces
{
    public interface IReportRepository
    {
        Task<IList<Reports>> GetAllReportsAsync();
        Task<Reports> GetReportByIdAsync(string id);
        Task<Reports> CreateReportAsync();
        Task ReportCompletedAsync(string id);
    }
}
