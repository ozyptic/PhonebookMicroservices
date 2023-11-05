using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Phonebook.Contact.API.Controllers.v1;
using Phonebook.Contact.Domain.Entities;
using Phonebook.Contact.Domain.Enums;
using Phonebook.Contact.Domain.VOs;
using Phonebook.Contact.Infrastracture.Interfaces;
using Phonebook.Contact.Infrastracture.Mappings;
using Phonebook.Shared.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Phonebook.Contact.UnitTest
{
    public class ContactTest
    {
        private readonly Mock<IContactsRepository> _contactRepoMock;
        private readonly Mock<IContactInfoRepository> _contactInfoRepoMock;
        private readonly IMapper _mapper;
        private readonly ContactsController _contactController;
        private readonly ContactInfoController _contactInfoController;

        public ContactTest()
        {
            _contactRepoMock = new Mock<IContactsRepository>();
            _contactInfoRepoMock = new Mock<IContactInfoRepository>();

            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<GlobalMappings>()).CreateMapper();

            _contactController = new ContactsController(_contactRepoMock.Object, _contactInfoRepoMock.Object, _mapper);
            _contactInfoController = new ContactInfoController(_contactInfoRepoMock.Object, _mapper);
        }


        [Fact]
        public async Task GetAllContactInfosAsList_TEST_Success()
        {
            _contactInfoRepoMock.Setup(x => x.GetAllContactInfosAsListAsync())
              .Returns(Task.FromResult(GetContactInfosFake("123ab456cd78e10")));

            var actionResult = await _contactInfoController.GetAllContactInfosAsList();
            var objectResult = (ObjectResult)actionResult;

            Assert.Equal(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsType<ResponseDataModel<IList<ContactInfoVO>>>(objectResult.Value);
        }

        [Fact]
        public async Task GetAllContactsAsList_TEST_Success()
        {
            _contactRepoMock.Setup(x => x.GetAllContactsAsListAsync())
              .Returns(Task.FromResult(GetContactsFake()));

            var actionResult = await _contactController.GetAllContactsAsList();
            var objectResult = (ObjectResult)actionResult;

            Assert.Equal(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsType<ResponseDataModel<IList<ContactInfoVO>>>(objectResult.Value);
        }

        [Fact]
        public async Task GetContactById_TEST_Success()
        {
            var fakeContactId = "111ab456cd78e10";

            _contactRepoMock.Setup(x => x.GetContactByIdAsync(It.IsAny<string>()))
              .Returns(Task.FromResult(GetContactById(fakeContactId)));

            var actionResult = await _contactController.GetContactById(fakeContactId);
            var objectResult = (ObjectResult)actionResult;
            var response = (ResponseDataModel<ContactVO>)objectResult.Value!;

            Assert.Equal(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.Equal(response.Data.Id, fakeContactId);
        }

        [Fact]
        public async Task GetContactById_TEST_NotFound()
        {
            var fakeContactId = "111ab456cd78e13";

            _contactRepoMock.Setup(x => x.GetContactByIdAsync(It.IsAny<string>()))
              .Returns(Task.FromResult(GetContactById(fakeContactId)));

            var actionResult = await _contactController.GetContactById(fakeContactId);
            var objectResult = (NotFoundResult)actionResult;

            Assert.Equal((int)System.Net.HttpStatusCode.NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task AddContact_TEST_Create()
        {
            var fakeContactId = "111ab456cd78e14";
            var fakeContact = GetContactById(fakeContactId);
            var model = _mapper.Map<ContactAddVO>(fakeContact);

            _contactRepoMock.Setup(x => x.AddContactAsync(It.IsAny<Contacts>()))
              .Returns(Task.FromResult(fakeContact));

            var actionResult = await _contactController.AddContact(model);

            var objectResult = (ObjectResult)actionResult;
            var response = (ResponseDataModel<ResultId<string>>)objectResult.Value!;

            Assert.Equal(objectResult.StatusCode, (int)System.Net.HttpStatusCode.Created);
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.Equal(response.Data.Id, fakeContactId);
        }

        [Fact]
        public Task AddContact_TEST_NotValid()
        {
            var model = new ContactAddVO
            {
                Name = "",
                LastName = "Danny",
                Company = "Test"
            };

            var validationContext = new ValidationContext(model);
            var results = model.Validate(validationContext);

            Assert.Contains(results, x => x.MemberNames.Contains(nameof(ContactAddVO.Name)));
            return Task.CompletedTask;
        }

        [Fact]
        public async Task AddContactInfo_TEST_Create()
        {
            var fakeContactInfoId = "123ab456cd78e15";
            var fakeContactInfo = GetContactInfoById(fakeContactInfoId);
            var model = _mapper.Map<ContactAddInfoVO>(fakeContactInfo);

            _contactInfoRepoMock.Setup(x => x.AddContactInfoAsync(It.IsAny<ContactInfo>()))
              .Returns(Task.FromResult(fakeContactInfo));

            var actionResult = await _contactInfoController.AddContactInfo(model);

            var objectResult = (ObjectResult)actionResult;
            var response = (ResponseDataModel<ResultId<string>>)objectResult.Value!;

            Assert.Equal(objectResult.StatusCode, (int)System.Net.HttpStatusCode.Created);
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.Equal(response.Data.Id, fakeContactInfoId);
        }

        [Fact]
        public Task AddContactInfo_TEST_NotValid()
        {
            var model = new ContactInfoVO
            {
                Id = "123ab456cd78e10",
                ContactId = "",
                ContactInfoType = (int)ContactInfoType.Location,
                Value = ""
            };

            var validationContext = new ValidationContext(model);
            var results = model.Validate(validationContext);

            var validationResults = results.ToList();
            Assert.Contains(validationResults, x => x.MemberNames.Contains(nameof(ContactInfoVO.Value)));
            Assert.Contains(validationResults, x => x.MemberNames.Contains(nameof(ContactInfoVO.ContactId)));
            return Task.CompletedTask;
        }

        [Fact]
        public async Task DeleteContact_TEST_Success()
        {
            _contactRepoMock.Setup(x => x.DeleteContactAsync(It.IsAny<string>()))
              .Returns(Task.FromResult(true));

            var actionResult = await _contactController.DeleteContact("111ab456cd78e10");
            var objectResult = (OkResult)actionResult;

            Assert.Equal((int)System.Net.HttpStatusCode.OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task DeleteContact_TEST_BadRequest()
        {
            _contactRepoMock.Setup(x => x.DeleteContactAsync(It.IsAny<string>()))
              .Returns(Task.FromResult(false));

            var actionResult = await _contactController.DeleteContact("111ab456cd78e10");
            var objectResult = (BadRequestResult)actionResult;

            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task DeleteContactDetails_TEST_Success()
        {
            _contactInfoRepoMock.Setup(x => x.DeleteContactInfoAsync(It.IsAny<string>()))
              .Returns(Task.FromResult(true));

            var actionResult = await _contactInfoController.DeleteContactInfo("111ab456cd78e10");
            var objectResult = (OkResult)actionResult;

            Assert.Equal((int)System.Net.HttpStatusCode.OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task DeleteContactInfo_TEST_BadRequest()
        {
            _contactInfoRepoMock.Setup(x => x.DeleteContactInfoAsync(It.IsAny<string>()))
              .Returns(Task.FromResult(false));

            var actionResult = await _contactInfoController.DeleteContactInfo("123ab456cd78e10");
            var objectResult = (BadRequestResult)actionResult;

            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        private Contacts GetContactById(string fakeContactId)
        {
            return GetContactsFake().FirstOrDefault(x => x.Id == fakeContactId)!;
        }

        private ContactInfo GetContactInfoById(string fakeContactInfoId)
        {
            return GetContactInfosFake("123ab456cd78e10").FirstOrDefault(x => x.Id == fakeContactInfoId)!;
        }

        private List<Contacts> GetContactsFake()
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

        private List<ContactInfo> GetContactInfosFake(string fakeContactId)
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