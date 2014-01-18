using StructureMap;

namespace Example.Common
{
    public interface IStartup : IApplicationStartup
    {
    }

    public interface IApplicationStartup 
    {
        void Execute(IContainer container);
    }
}