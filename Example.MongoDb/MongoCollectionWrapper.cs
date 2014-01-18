using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Example.Data.MongoDb
{
    public class MongoCollectionWrapper<T> : IMongoCollection<T>
    {
        readonly MongoCollection<T> _collection;

        public MongoCollectionWrapper(MongoCollection<T> collection)
        {
            _collection = collection;
        }

        public WriteConcernResult Insert(T document)
        {
            return _collection.Insert(document);
        }

        public WriteConcernResult Remove(IMongoQuery query)
        {
            return _collection.Remove(query);
        }

        public T FindOneAs(IMongoQuery query)
        {
            return _collection.FindOneAs<T>(query);
        }

        public IQueryable<T> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public WriteConcernResult Update(IMongoQuery query, IMongoUpdate update)
        {
            return _collection.Update(query, update , UpdateFlags.None);
        }

        public void Save(T objectToSave)
        {
            _collection.Save(objectToSave);
        }
    }
}