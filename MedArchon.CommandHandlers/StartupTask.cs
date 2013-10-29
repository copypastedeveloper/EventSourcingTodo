using MedArchon.CommandBus;
using MedArchon.Common;
using StructureMap;

namespace MedArchon.CommandHandlers
{
    public class StartupTask : IStartup
    {
        public void Execute(IContainer container)
        {
            container.Configure(x =>
            {
                x.Scan(scanner =>
                {
                    scanner.TheCallingAssembly();
                    scanner.AddAllTypesOf<ICommandHandler>();
                });
            });
        }
    }
}