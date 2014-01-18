using System;
using System.Linq;
using Example.Web.Denormalizer;
using Example.Web.ServiceContracts;
using log4net;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Example.Data.MongoDb
{
    public class MongoRepository : IRepository, IViewModelData
    {
        readonly MongoDatabase _db;
        readonly ILog _log;
        internal const string CannotRemoveInfoMessage = "Object {0} with ID of {1} could not be removed because it was not found.";

        public MongoRepository(MongoDatabase db, ILog log)
        {
            _db = db;
            _log = log;
        }

        public IQueryable<T> Query<T>()
        {
            return GetCollection<T>().AsQueryable();
        }

        public T GetById<T>(Guid id)
        {
            return GetCollection<T>().FindOneAs(new QueryDocument("_id", id.ToString()));
        }

        public void Insert<T>(T itemToInsert)
        {
            GetCollection<T>().Insert(itemToInsert);
        }

        public void Remove<T>(Guid id)
        {
            Remove(id, GetCollection<T>());
        }

        public void Update<T>(Guid id, SetOperationList<T> setOperations)
        {
            var updateBuilder = new UpdateBuilder<T>();

            foreach (var expression in setOperations)
            {
                updateBuilder.Set(expression.Key, expression.Value);
            }

            GetCollection<T>().Update(new QueryDocument("_id", id.ToString()), updateBuilder);
        }

        public void Upsert<T>(T objectToSave)
        {
            GetCollection<T>().Save(objectToSave);
        }

        internal void Remove<T>(Guid id, IMongoCollection<T> collection)
        {
            WriteConcernResult concernResult = collection.Remove(new QueryDocument("_id", id.ToString()));

            if (concernResult.DocumentsAffected == 0)
            {
                _log.InfoFormat(CannotRemoveInfoMessage, typeof(T), id);
            }
        }

        IMongoCollection<T> GetCollection<T>()
        {
            return new MongoCollectionWrapper<T>(_db.GetCollection<T>(typeof(T).Name));
        }
    }
}
