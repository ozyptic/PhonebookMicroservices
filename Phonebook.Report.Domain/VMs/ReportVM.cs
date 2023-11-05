using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Report.Domain.VMs
{
    public class ReportViewModel
    {
        public string Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public IList<ReportInfoVM> ReportInfos { get; set; }
    }
}
