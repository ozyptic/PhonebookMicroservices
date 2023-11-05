using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Phonebook.Report.Domain.Dtos;
using Phonebook.Report.Domain.Entities;

namespace Phonebook.Report.Infrastructure.Mappings
{
    public class ReportMappings : Profile
    {
        public ReportMappings()
        {
            CreateMap<Reports, ReportDto>().ReverseMap();
            CreateMap<Reports, ReportIxDto>().ReverseMap();
            CreateMap<ReportDetail, ReportDetailDto>().ReverseMap();
        }
    }
}
