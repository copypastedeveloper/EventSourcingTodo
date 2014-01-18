using System.Configuration;
using Example.Common;
using log4net;
using MassTransit;
using StructureMap;

namespace Example.Web.Denormalizer.Host
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
                    scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.GetName().Name.StartsWith("Example"));
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
