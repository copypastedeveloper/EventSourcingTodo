using Example.CommandBus;
using Example.Common;
using StructureMap;

namespace Example.CommandHandlers
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