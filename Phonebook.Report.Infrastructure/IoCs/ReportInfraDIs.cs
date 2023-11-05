using Microsoft.Extensions.DependencyInjection;
using Phonebook.Report.Infrastructure.Interfaces;
using Phonebook.Report.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Report.Infrastructure.IoCs
{
    public static class ReportInfraDIs
    {
        public static IServiceCollection AddReportInfraDIs(this IServiceCollection services)
        {
            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<IReportDetailRepository, ReportDetailRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
