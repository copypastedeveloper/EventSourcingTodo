using System.Configuration;
using MedArchon.Common;
using log4net;
using MassTransit;
using StructureMap;

namespace MedArchon.Web.Denormalizer.Host
{
    public class StartupTask : IApplicationStartup
    {
        public void Execute(IContainer container)
        {
            log4net.Config.XmlConfigurator.Configure(); 

            container.Configure(x =>
            {
                x.Scan(scanner =>
                {
                    scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.GetName().Name.StartsWith("MedArchon"));
                    scanner.AddAllTypesOf(typeof (IStartup));
                    scanner.AddAllTypesOf(typeof(IConsumer));
                    scanner.SingleImplementationsOfInterface();
                    scanner.WithDefaultConventions();
                });
                x.For<ILog>().Use(LogManager.GetLogger(typeof(StartupTask)));
            });

            foreach (var task in container.GetAllInstances<IStartup>())
            {
                task.Execute(container);
            }

            // must be done out of the normal configuration due to the dependence on the 
            // scan for consumers having been completed.
            container.Inject(typeof(IServiceBus),
                ServiceBusFactory.New(sbc =>
                {
                    sbc.UseRabbitMq();
                    sbc.UseControlBus();
                    sbc.ReceiveFrom(ConfigurationManager.AppSettings["WebDenormalizerMessageQueue"]);
                    sbc.Subscribe(subs => subs.LoadFrom(container));
                }));
        }
    }
}
