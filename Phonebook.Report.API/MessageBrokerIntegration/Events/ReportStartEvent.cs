using EventBus.Base.Events;

namespace Phonebook.Report.API.MessageBrokerIntegration.Events
{
    public class ReportStartEvent : IntegrationEvent
    {
        public ReportStartEvent() { }

        public ReportStartEvent(string reportId)
        {
            ReportId = reportId;
        }

        public string ReportId { get; set; }
    }
}
