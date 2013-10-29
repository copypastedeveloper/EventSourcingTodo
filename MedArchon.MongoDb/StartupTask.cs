using System;
using System.Configuration;
using MedArchon.Common;
using MedArchon.Web.Denormalizer;
using MedArchon.Web.ServiceContracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Driver;
using StructureMap;

namespace MedArchon.Data.MongoDb
{
    public class StartupTask : IStartup
    {
        public void Execute(IContainer container)
        {
            var conventions = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true), 
                new NamedIdMemberConvention("Id"),
                new MemberSerializationOptionsConvention(typeof(Guid), new RepresentationSerializationOptions(BsonType.String)),
                //new MemberSerializationOptionsConvention(typeof(DateTime), new DateTimeSerializationOptions(DateTimeKind.Utc))
            };
            ConventionRegistry.Register("DefaultConventions", conventions, type => true);
            
            var client = new MongoClient(ConfigurationManager.AppSettings["MongoServerConnectionString"]);
            var server = client.GetServer();
            var db = server.GetDatabase(ConfigurationManager.AppSettings["MongoDatabaseName"]);

            container.Configure(x =>
            {
                x.For<MongoDatabase>().Singleton().Use(db);
                x.For<MongoServer>().Singleton().Use(server);
                x.For<IRepository>().Use<MongoRepository>();
                x.For<IViewModelData>().Use<MongoRepository>();
            });
        }
    }
}