using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Report.Domain.VMs
{
    public class ReportDetailVM
    {
        public string Location { get; set; }
        public int ContactCount { get; set; }
        public int PhoneNumberCount { get; set; }
    }
}
