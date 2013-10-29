using System;
using AutoMapper;
using MedArchon.Common;
using StructureMap;

namespace MedArchon.Web.Denormalizer
{
    public class StartupTask : IStartup
    {
        internal static readonly Guid AssemblyGuidNamespace = Guid.Parse("7c101ce9-6085-4382-8e17-29188d4916e8");

        public void Execute(IContainer container)
        {
            container.Configure(x =>
            {
                x.For<IMappingEngine>().Singleton().Use(Mapper.Engine);
            });

            Mapper.AddProfile<EventToViewModelMappingProfile>();
        }
    }
}
