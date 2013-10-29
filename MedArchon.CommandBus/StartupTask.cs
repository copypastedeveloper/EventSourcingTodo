using MedArchon.Common;
using StructureMap;

namespace MedArchon.CommandBus
{
    public class StartupTask : IApplicationStartup
    {
        public void Execute(IContainer container)
        {
            ObjectFactory.Configure(x => x.Scan(scanner =>
            {
                scanner.AssembliesFromApplicationBaseDirectory();
                scanner.AddAllTypesOf<IStartup>();
                scanner.SingleImplementationsOfInterface();
                scanner.WithDefaultConventions();
            }));

            foreach (var task in ObjectFactory.GetAllInstances<IStartup>())
            {
                task.Execute(ObjectFactory.Container);
            }
        }
    }
}