using EventStore;
using MedArchon.Common;
using StructureMap;

namespace MedArchon.Data.EventStore
{
    public class StartupTask : IStartup
    {
        public void Execute(IContainer container)
        {
            var eventStore = Wireup.Init()
                .UsingInMemoryPersistence()
                .InitializeStorageEngine()
                .UsingJsonSerialization()
                .UsingSynchronousDispatchScheduler()
                .Build();
            
            container.Configure(x => x.For<IStoreEvents>().Use(eventStore));
        }
    }
}