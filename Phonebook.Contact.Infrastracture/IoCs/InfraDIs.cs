using Microsoft.Extensions.DependencyInjection;
using Phonebook.Contact.Infrastracture.Interfaces;
using Phonebook.Contact.Infrastracture.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Contact.Infrastracture.IoCs
{
    public static class InfraDIs
    {
        public static IServiceCollection AddInfraDIs(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IContactsRepository, ContactsRepository>();
            services.AddScoped<IContactInfoRepository, ContactInfoRepository>();
            return services;
        }
    }
}
