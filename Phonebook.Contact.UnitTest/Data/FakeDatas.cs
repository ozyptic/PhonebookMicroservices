using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phonebook.Contact.Domain.Entities;
using Phonebook.Contact.Domain.Enums;

namespace Phonebook.Contact.UnitTest.Data
{
    public static class FakeDatas
    {
        public static string FakeContactId = "111ab456cd78e10";
        public static string FakeContactInfoId = "123ab456cd78e15";
        public static Contacts GetContactById(string fakeContactId)
        {
            return GetContactsFake().FirstOrDefault(x => x.Id == fakeContactId)!;
        }

        public static ContactInfo GetContactInfoById(string fakeContactInfoId)
        {
            return GetContactInfosFake("123ab456cd78e10").FirstOrDefault(x => x.Id == fakeContactInfoId)!;
        }

        public static List<Contacts> GetContactsFake()
        {
            return new List<Contacts>
            {
                new Contacts
                {
                    Id="111ab456cd78e10",
                    Name = "Jack",
                    LastName = "Daniels",
                    Company = "Blizzard",
                },
                new Contacts
                {
                    Id="111ab456cd78e11",
                    Name = "Joseph",
                    LastName = "Dieder",
                    Company = "Siemens",
                },
            };
        }

        public static List<ContactInfo> GetContactInfosFake(string fakeContactId)
        {
            return new List<ContactInfo>
            {
                new ContactInfo
                {
                    Id = "123ab456cd78e10",
                    ContactId = fakeContactId,
                    Value = "05301112233",
                    ContactInfoType = ContactInfoType.Phone,
                },
                new ContactInfo
                {
                    Id = "123ab456cd78e11",
                    ContactId = fakeContactId,
                    Value = "05359998877",
                    ContactInfoType = ContactInfoType.Phone,
                }
            };
        }
    }
}
