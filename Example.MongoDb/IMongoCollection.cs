using System.Linq;
using MongoDB.Driver;

namespace Example.Data.MongoDb
{
    public interface IMongoCollection<T>
    {
        WriteConcernResult Insert(T document);

        WriteConcernResult Remove(IMongoQuery query);

        T FindOneAs(IMongoQuery query);

        IQueryable<T> AsQueryable();

        WriteConcernResult Update(IMongoQuery query, IMongoUpdate update);
        
        void Save(T objectToSave);
    }
}
