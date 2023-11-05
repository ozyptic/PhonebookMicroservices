using AutoMapper;
using EventBus.Base.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Phonebook.Report.API.Controllers.v1;
using Phonebook.Report.API.MessageBrokerIntegration.Events;
using Phonebook.Report.Domain.Dtos;
using Phonebook.Report.Domain.Entities;
using Phonebook.Report.Infrastructure.Interfaces;
using Phonebook.Report.Infrastructure.Mappings;
using Phonebook.Report.UnitTest.Data;
using Phonebook.Shared.Models;
using Xunit;

namespace Phonebook.Report.UnitTest
{
    public class ReportTest
    {
        private readonly Mock<IReportRepository> _reportRepoMock;
        private readonly Mock<IEventBus> _serviceBusMock;
        private readonly ReportsController _reportController;

        public ReportTest()
        {
            _reportRepoMock = new Mock<IReportRepository>();
            Mock<IReportDetailRepository> reportDetailRepoMock = new();
            _serviceBusMock = new Mock<IEventBus>();

            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<ReportMappings>()).CreateMapper();
            _reportController = new ReportsController(_serviceBusMock.Object, _reportRepoMock.Object, mapper, reportDetailRepoMock.Object);
        }

        [Fact]
        public async Task GetAllReports_TEST_Success()
        {
            _reportRepoMock.Setup(x => x.GetAllReportsAsync())
              .Returns(Task.FromResult(FakeDatas.GetReportsFake()));

            var actionResult = await _reportController.GetAllReports();
            var objectResult = (ObjectResult)actionResult;

            Assert.Equal(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsType<ResponseDataModel<IList<ReportIxDto>>>(objectResult.Value);
        }


        [Fact]
        public async Task ReportCreated_TEST_Create()
        {
            var fakeReport = FakeDatas.GetReportById(FakeDatas.FakeReportId);

            _reportRepoMock.Setup(x => x.CreateReportAsync())
              .Returns(Task.FromResult(fakeReport));

            _serviceBusMock.Setup(x => x.Publish(It.IsAny<ReportStartEvent>()));

            var actionResult = await _reportController.CreateReport();

            var objectResult = (ObjectResult)actionResult;
            var response = (ResponseDataModel<ResultId<string>>)objectResult.Value!;

            Assert.Equal(objectResult.StatusCode, (int)System.Net.HttpStatusCode.Created);
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.Equal(response.Data.Id, FakeDatas.FakeReportId);
        }

        [Fact]
        public async Task CreateReport_TEST_BadRequest()
        {
            _reportRepoMock.Setup(x => x.CreateReportAsync())
              .Returns(Task.FromResult((Reports?)null)!);

            var actionResult = await _reportController.CreateReport();

            var objectResult = (BadRequestResult)actionResult;

            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetReportById_TEST_Success()
        {
            _reportRepoMock.Setup(x => x.GetReportByIdAsync(It.IsAny<string>()))
              .Returns(Task.FromResult(FakeDatas.GetReportById(FakeDatas.FakeReportId)));

            var actionResult = await _reportController.GetReportById(FakeDatas.FakeReportId);

            var objectResult = (ObjectResult)actionResult;
            var response = (ResponseDataModel<ReportDto>)objectResult.Value!;

            Assert.Equal(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.Equal(response.Data.Id, FakeDatas.FakeReportId);
        }

        [Fact]
        public async Task GetReportById_TEST_NotFound()
        {
            _reportRepoMock.Setup(x => x.GetReportByIdAsync(It.IsAny<string>()))
              .Returns(Task.FromResult((Reports?)null)!);

            var actionResult = await _reportController.GetReportById(FakeDatas.FakeReportId);

            var objectResult = (NotFoundResult)actionResult;

            Assert.Equal((int)System.Net.HttpStatusCode.NotFound, objectResult.StatusCode);
        }
    }
}