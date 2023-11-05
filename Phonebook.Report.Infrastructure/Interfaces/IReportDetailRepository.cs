using Phonebook.Report.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Report.Infrastructure.Interfaces
{
    public interface IReportDetailRepository
    {
        Task<IList<ReportDetail>> GetDetailsByReportIdAsync(string reportId);
        Task CreateReportDetailsAsync(IList<ReportDetail> addReportDetails);
    }
}
