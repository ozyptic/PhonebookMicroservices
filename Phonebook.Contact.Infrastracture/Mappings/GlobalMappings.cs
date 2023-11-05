using AutoMapper;
using Phonebook.Contact.Domain.Entities;
using Phonebook.Contact.Domain.VOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Contact.Infrastracture.Mappings
{
    public class GlobalMappings : Profile
    {
        public GlobalMappings()
        {
            CreateMap<Contacts, ContactVO>().ReverseMap();
            CreateMap<Contacts, ContactIXVO>().ReverseMap();
            CreateMap<Contacts, ContactUpdateVO>().ReverseMap();
            CreateMap<Contacts, ContactAddVO>().ReverseMap();
            CreateMap<ContactInfo, ContactInfoVO>().ReverseMap();
            CreateMap<ContactInfo, ContactAddInfoVO>().ReverseMap();
        }
    }
}
