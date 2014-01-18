using System;
using MongoDB.Driver;

namespace Example.Common.MongoDB
{
    public static class MongoCollectionExtensions
    {
        public static MongoCursor<T> FindById<T>(this MongoCollection<T> mongoCollection, Guid id)
        {
            return mongoCollection.Find(new QueryDocument("_id", id.ToString()));
        }
    }
}
