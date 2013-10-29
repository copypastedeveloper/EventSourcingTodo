using MedArchon.Todo.Domain.Common;

namespace MedArchon.Data.EventStore
{
    public interface IEntityFactory
    {
        TEntity Create<TEntity>() where TEntity : IEntity;
    }
}