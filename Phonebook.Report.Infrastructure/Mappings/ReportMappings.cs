using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Phonebook.Report.Domain.Entities;
using Phonebook.Report.Domain.VMs;

namespace Phonebook.Report.Infrastructure.Mappings
{
    public class ReportMappings : Profile
    {
        public ReportMappings()
        {
            CreateMap<Reports, ReportVM>().ReverseMap();
            CreateMap<Reports, ReportIxVM>().ReverseMap();
            CreateMap<ReportDetail, ReportDetailVM>().ReverseMap();
        }
    }
}
