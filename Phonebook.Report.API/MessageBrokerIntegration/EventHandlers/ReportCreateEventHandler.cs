using EventBus.Base.Abstraction;
using Phonebook.Report.API.MessageBrokerIntegration.Events;
using Phonebook.Report.Domain.Entities;
using Phonebook.Report.Infrastructure.Interfaces;

namespace Phonebook.Report.API.MessageBrokerIntegration.EventHandlers
{
    public class ReportCreateEventHandler : IIntegrationEventHandler<ReportCreateEvent>
    {
        private readonly IReportDetailRepository _reportDetailRepository;
        private readonly IReportRepository _reportRepository;
        private readonly ILogger<ReportCreateEvent> logger;

        public ReportCreateEventHandler(
            IReportDetailRepository reportDetailRepository,
            IReportRepository reportRepository,
            ILogger<ReportCreateEvent> logger)
        {
            _reportRepository = reportRepository;
            _reportDetailRepository = reportDetailRepository;
            this.logger = logger;
        }

        public async Task Handle(ReportCreateEvent @event)
        {
            logger.LogInformation("@ RabbitMQ broker handling: {IntegrationEventId} at PhoneBook.Report.API - ({@IntegrationEvent})", @event.ReportId, @event);

            var report = await _reportRepository.GetReportByIdAsync(@event.ReportId);
            if (report == null) return;
            try
            {
                await _reportRepository.ReportCompletedAsync(report.Id);
                logger.LogInformation("@ Report Completed : | {IntegrationEventId} | Report Id : " + report.Id);
            }
            catch (Exception ex)
            {
                logger.LogInformation("@ Report Not Completed : | {IntegrationEventId} | Error is : " + ex.Message);
            }
            if (@event == null) return;
            var details = @event.Details
                .Select(x => new ReportDetail(@event.ReportId, x.Location, x.ContactCount, x.PhoneNumberCount))
                .ToList();

            await _reportDetailRepository.CreateReportDetailsAsync(details);
        }
    }
}
