using EventBus.Base.Events;
using Phonebook.Report.Domain.Dtos;

namespace Phonebook.Report.API.MessageBrokerIntegration.Events
{
    public class ReportCreateEvent : IntegrationEvent
    {
        public string? ReportId { get; set; }
        public IList<ReportDetailDto>? Details { get; set; }

    }
}
