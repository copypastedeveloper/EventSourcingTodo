using System.Configuration;
using MassTransit;
using MassTransit.Log4NetIntegration;
using MedArchon.Common;
using StructureMap;

namespace MedArchon.EventPublisher
{
    public class StartupTask : IStartup
    {
        public void Execute(IContainer container)
        {
            ObjectFactory.Configure(x =>
            {
                var bus = ServiceBusFactory.New(sbc =>
                {
                    sbc.UseRabbitMq();
                    sbc.UseControlBus();
                    sbc.ReceiveFrom(ConfigurationManager.AppSettings["EventPublisherMessageQueue"]);
                    sbc.UseLog4Net();
                });

                x.For<IServiceBus>().Use(bus);
            });
        }
    }
}