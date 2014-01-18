namespace MedArchon.Todo.Domain.Common
{
    public interface ISnapshotable<TMemento>
    {
        TMemento GetMemento();
        void Hydrate(TMemento memento);
    }
}