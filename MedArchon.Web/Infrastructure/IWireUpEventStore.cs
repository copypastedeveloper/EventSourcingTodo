using System;
using EventStore;

namespace MedArchon.Web.Infrastructure
{
    public interface IWireUpEventStore
    {
        Wireup Execute(Wireup wireUp);
    }

    public class DelegateWireUpEventStore : IWireUpEventStore
    {
        readonly Func<Wireup, Wireup> _configFunction;

        public DelegateWireUpEventStore(Func<Wireup,Wireup> configFunction )
        {
            _configFunction = configFunction;
        }

        public Wireup Execute(Wireup wireUp)
        {
            return _configFunction.Invoke(wireUp);
        }
    }
}