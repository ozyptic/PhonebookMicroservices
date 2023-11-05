using AutoMapper;
using Phonebook.Contact.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phonebook.Contact.Domain.Dtos;

namespace Phonebook.Contact.Infrastracture.Mappings
{
    public class GlobalMappings : Profile
    {
        public GlobalMappings()
        {
            CreateMap<Contacts, ContactDto>().ReverseMap();
            CreateMap<Contacts, ContactIXDto>().ReverseMap();
            CreateMap<Contacts, ContactUpdateDto>().ReverseMap();
            CreateMap<Contacts, ContactAddDto>().ReverseMap();
            CreateMap<ContactInfo, ContactInfoDto>().ReverseMap();
            CreateMap<ContactInfo, ContactAddInfoDto>().ReverseMap();
        }
    }
}
