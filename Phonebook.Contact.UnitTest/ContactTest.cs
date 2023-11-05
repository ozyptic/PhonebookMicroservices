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
using Phonebook.Contact.UnitTest.Data;
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
              .Returns(Task.FromResult(FakeDatas.GetContactInfosFake("123ab456cd78e10")));

            var actionResult = await _contactInfoController.GetAllContactInfosAsList();
            var objectResult = (ObjectResult)actionResult;

            Assert.Equal(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsType<ResponseDataModel<IList<ContactInfoVo>>>(objectResult.Value);
        }

        [Fact]
        public async Task GetAllContactsAsList_TEST_Success()
        {
            _contactRepoMock.Setup(x => x.GetAllContactsAsListAsync())
              .Returns(Task.FromResult(FakeDatas.GetContactsFake()));

            var actionResult = await _contactController.GetAllContactsAsList();
            var objectResult = (ObjectResult)actionResult;

            Assert.Equal(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsType<ResponseDataModel<IList<ContactInfoVo>>>(objectResult.Value);
        }

        [Fact]
        public async Task GetContactById_TEST_Success()
        {
            _contactRepoMock.Setup(x => x.GetContactByIdAsync(It.IsAny<string>()))
              .Returns(Task.FromResult(FakeDatas.GetContactById(FakeDatas.FakeContactId)));

            var actionResult = await _contactController.GetContactById(FakeDatas.FakeContactId);
            var objectResult = (ObjectResult)actionResult;
            var response = (ResponseDataModel<ContactVo>)objectResult.Value!;

            Assert.Equal(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.Equal(response.Data.Id, FakeDatas.FakeContactId);
        }

        [Fact]
        public async Task GetContactById_TEST_NotFound()
        {
            _contactRepoMock.Setup(x => x.GetContactByIdAsync(It.IsAny<string>()))
              .Returns(Task.FromResult(FakeDatas.GetContactById(FakeDatas.FakeContactId)));

            var actionResult = await _contactController.GetContactById(FakeDatas.FakeContactId);
            var objectResult = (NotFoundResult)actionResult;

            Assert.Equal((int)System.Net.HttpStatusCode.NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task AddContact_TEST_Create()
        {
            var fakeContact = FakeDatas.GetContactById(FakeDatas.FakeContactId);
            var model = _mapper.Map<ContactAddVo>(fakeContact);

            _contactRepoMock.Setup(x => x.AddContactAsync(It.IsAny<Contacts>()))
              .Returns(Task.FromResult(fakeContact));

            var actionResult = await _contactController.AddContact(model);

            var objectResult = (ObjectResult)actionResult;
            var response = (ResponseDataModel<ResultId<string>>)objectResult.Value!;

            Assert.Equal(objectResult.StatusCode, (int)System.Net.HttpStatusCode.Created);
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.Equal(response.Data.Id, FakeDatas.FakeContactId);
        }

        [Fact]
        public Task AddContact_TEST_NotValid()
        {
            var model = new ContactAddVo
            {
                Name = "",
                LastName = "Danny",
                Company = "Test"
            };

            var validationContext = new ValidationContext(model);
            var results = model.Validate(validationContext);

            Assert.Contains(results, x => x.MemberNames.Contains(nameof(ContactAddVo.Name)));
            return Task.CompletedTask;
        }

        [Fact]
        public async Task AddContactInfo_TEST_Create()
        {
            var fakeContactInfo = FakeDatas.GetContactInfoById(FakeDatas.FakeContactInfoId);
            var model = _mapper.Map<ContactAddInfoVo>(fakeContactInfo);

            _contactInfoRepoMock.Setup(x => x.AddContactInfoAsync(It.IsAny<ContactInfo>()))
              .Returns(Task.FromResult(fakeContactInfo));

            var actionResult = await _contactInfoController.AddContactInfo(model);

            var objectResult = (ObjectResult)actionResult;
            var response = (ResponseDataModel<ResultId<string>>)objectResult.Value!;

            Assert.Equal(objectResult.StatusCode, (int)System.Net.HttpStatusCode.Created);
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.Equal(response.Data.Id, FakeDatas.FakeContactInfoId);
        }

        [Fact]
        public Task AddContactInfo_TEST_NotValid()
        {
            var model = new ContactInfoVo
            {
                Id = "123ab456cd78e10",
                ContactId = "",
                ContactInfoType = (int)ContactInfoType.Location,
                Value = ""
            };

            var validationContext = new ValidationContext(model);
            var results = model.Validate(validationContext);

            var validationResults = results.ToList();
            Assert.Contains(validationResults, x => x.MemberNames.Contains(nameof(ContactInfoVo.Value)));
            Assert.Contains(validationResults, x => x.MemberNames.Contains(nameof(ContactInfoVo.ContactId)));
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

        
    }
}