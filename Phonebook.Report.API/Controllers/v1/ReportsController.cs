using System.Diagnostics;
using AutoMapper;
using EventBus.Base.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phonebook.Report.Infrastructure.Interfaces;
using Phonebook.Shared.Models;
using System.Net;
using Phonebook.Report.API.MessageBrokerIntegration.Events;
using Phonebook.Report.Domain.Dtos;

namespace Phonebook.Report.API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        private readonly IReportDetailRepository _reportDetailRepository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;
        public ReportsController(
            IEventBus eventBus,
            IReportRepository reportRepository,
            IMapper mapper,
            IReportDetailRepository reportDetailsRepository)
        {
            _eventBus = eventBus;
            _mapper = mapper;
            _reportRepository = reportRepository;
            _reportDetailRepository = reportDetailsRepository;
        }


        [HttpGet("GetAllReports")]
        [ProducesResponseType(typeof(ResponseDataModel<IList<ReportIxDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllReports()
        {
            var list = await _reportRepository.GetAllReportsAsync();

            IList<ReportIxDto>? reports = null;
            if (list.Any())
            {
                reports = _mapper.Map<IList<ReportIxDto>>(list);
            }

            Debug.Assert(reports != null, nameof(reports) + " != null");
            return ResponseDataModel<IList<ReportIxDto>>.Success(reports, (int)HttpStatusCode.OK);
        }

        [HttpPost("CreateReport")]
        [ProducesResponseType(typeof(ResponseDataModel<ResultId<string>>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateReport()
        {
            var newReport = await _reportRepository.CreateReportAsync();

            if (newReport == null) return BadRequest();

            var reportStartedEventModel = new ReportStartEvent(newReport.Id);
            _eventBus.Publish(reportStartedEventModel);

            var result = new ResultId<string?> { Id = newReport.Id };
            return ResponseDataModel<ResultId<string>>.Success(result!, (int)HttpStatusCode.Created);
        }


        [HttpGet("GetReportByIdAsync/{id}")]
        [ProducesResponseType(typeof(ResponseDataModel<ReportDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetReportById(string? id)
        {
            var report = await _reportRepository.GetReportByIdAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<ReportDto>(report);

            var details = await _reportDetailRepository.GetDetailsByReportIdAsync(report.Id);
            if (details != null && details.Any())
            {
                model.ReportDetails = _mapper.Map<IList<ReportDetailDto>>(details);
            }

            return ResponseDataModel<ReportDto>.Success(model, (int)HttpStatusCode.OK);
        }
    }
}
