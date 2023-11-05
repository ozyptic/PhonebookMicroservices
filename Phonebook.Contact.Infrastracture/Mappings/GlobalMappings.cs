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
            CreateMap<Contacts, ContactVo>().ReverseMap();
            CreateMap<Contacts, ContactIxvo>().ReverseMap();
            CreateMap<Contacts, ContactUpdateVo>().ReverseMap();
            CreateMap<Contacts, ContactAddVo>().ReverseMap();
            CreateMap<ContactInfo, ContactInfoVo>().ReverseMap();
            CreateMap<ContactInfo, ContactAddInfoVo>().ReverseMap();
        }
    }
}
