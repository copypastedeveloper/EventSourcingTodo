namespace Example.Todo.Domain.Common
{
    public interface ISnapshotable<TMemento>
    {
        TMemento GetMemento();
        void Hydrate(TMemento memento);
        int MaxAllowedRevisionsBetweenSnapshots { get; }
    }
}