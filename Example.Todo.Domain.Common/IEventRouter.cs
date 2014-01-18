
namespace Example.Todo.Domain.Common
{
    public interface IEventRouter
    {
        void Register(IEntity entity);
        void Dispatch(object eventMessage);
    }
}