using EventStore;
using Example.Common;
using StructureMap;

namespace Example.Data.EventStore
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