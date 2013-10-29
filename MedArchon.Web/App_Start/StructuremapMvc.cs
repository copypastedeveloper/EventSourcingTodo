// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructuremapMvc.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using EventStore;
using EventStore.Dispatcher;
using EventStore.Persistence.SqlPersistence.SqlDialects;
using MedArchon.Common;
using MedArchon.Data.EventStore.ServiceContracts;
using MedArchon.Web.App_Start;
using MedArchon.Web.App_Start.DependencyResolution;
using MedArchon.Web.Infrastructure;
using log4net;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(StructuremapMvc), "AutoStart")]

namespace MedArchon.Web.App_Start
{
    public static class StructuremapMvc
    {

        public static void AutoStart()
        {
            var eventStorePersistanceWireUp =
                new DelegateWireUpEventStore(wireup =>
                    wireup.UsingSqlPersistence("EventStore") // connection string is in app.config
                        .WithDialect(new MsSqlDialect())
                        //.EnlistInAmbientTransaction() // two-phase commit
                        .InitializeStorageEngine() //sets up the db
                        .UsingJsonSerialization()
                        .Compress());

            Start(eventStorePersistanceWireUp);
        }

        public static void Start(IWireUpEventStore eventStorePersistanceWireUp)
        {
            log4net.Config.XmlConfigurator.Configure(); 
            
            CreateMaps();

            ObjectFactory.Configure(containerToConfigure =>
            {
                containerToConfigure.Scan(scanner =>
                {
                    scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.Contains("MedArchon"));
                    scanner.AddAllTypesOf<IStartup>();
                    scanner.WithDefaultConventions();
                    scanner.SingleImplementationsOfInterface();
                });
                containerToConfigure.For<ILog>().Use(LogManager.GetLogger(typeof(StructuremapMvc)));
                containerToConfigure.For<IMappingEngine>().Use(Mapper.Engine);
                containerToConfigure.For<IIdentity>().Use(() => HttpContext.Current.User.Identity);
            });

            var container = ObjectFactory.Container;

            foreach (var task in container.GetAllInstances<IStartup>())
            {
                task.Execute(container);
            }

            ObjectFactory.Configure(containerToConfigure =>
            {
                containerToConfigure.For<IStoreEvents>().Singleton().Use(
                    eventStorePersistanceWireUp.Execute(Wireup.Init())
                        .UsingSynchronousDispatchScheduler(
                            new DelegateMessageDispatcher(x =>
                            {
                                var eventPublisher = container.GetInstance<IEventPublisher>();
                                foreach (var eventMessage in x.Events)
                                {
                                    eventPublisher.PublishEvent(eventMessage.Body);
                                }
                            }))
                        .Build());
            });

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapDependencyResolver(container);
        }

        internal static void CreateMaps()
        {
            Mapper.AddProfile<CommonAutoMapperProfile>();
        }
    }
}