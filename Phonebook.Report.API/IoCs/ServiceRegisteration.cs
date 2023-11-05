using EventBus.Base.Abstraction;
using EventBus.Base;
using RabbitMQ.Client;
using EventBus.Factory;
using Phonebook.Report.API.MessageBrokerIntegration.EventHandlers;
using Phonebook.Report.API.MessageBrokerIntegration.Events;

namespace Phonebook.Report.API.IoCs
{
    public static class ServiceRegisteration
    {
        public static void ConfigureRabbitMQ(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(sp =>
            {
                EventBusConfig config = new()
                {
                    ConnectionRetryCount = 5,
                    EventNameSuffix = builder.Configuration.GetValue<string>("RabbitMQConfig:EventNameSuffix"),
                    SubscriberClientAppName = builder.Configuration.GetValue<string>("RabbitMQConfig:SubscriberClientAppName"),
                    Connection = new ConnectionFactory()
                    {
                        HostName = builder.Configuration.GetValue<string>("RabbitMQConfig:HostName"),
                        Port = builder.Configuration.GetValue<int>("RabbitMQConfig:Port")
                    },
                    EventBusType = EventBusType.RabbitMQ,

                };

                return EventBusFactory.Create(config, sp);
            });

            builder.Services.AddTransient<ReportCreateEventHandler>();
        }

        public static void ConfigureEventBusForSubscription(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<ReportCreateEvent, ReportCreateEventHandler>();
        }
    }
}
