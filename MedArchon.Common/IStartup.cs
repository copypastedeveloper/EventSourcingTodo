using StructureMap;

namespace MedArchon.Common
{
    public interface IStartup : IApplicationStartup
    {
    }

    public interface IApplicationStartup 
    {
        void Execute(IContainer container);
    }
}