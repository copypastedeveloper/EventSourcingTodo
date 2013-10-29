using System;
using System.Linq;

namespace MedArchon.Web.Denormalizer
{
    public interface IRepository
    {
        IQueryable<T> Query<T>();
        T GetById<T>(Guid id);
        void Insert<T>(T itemToInsert);
        void Remove<T>(Guid id);
        void Update<T>(Guid id, SetOperationList<T> setOperations);
        void Upsert<T>(T objectToSave);
    }
}