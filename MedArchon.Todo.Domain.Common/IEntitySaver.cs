namespace MedArchon.Todo.Domain.Common
{
    public interface IEntitySaver
    {
        void Save<TEntity>(TEntity entity) where TEntity : IEntity;
    }
}