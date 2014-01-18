using System;
using Example.Todo.Domain.Common;
using Example.Todo.Domain.Events;

namespace Example.Todo.Domain
{
    public class Task : DomainEntity, ISnapshotable<TaskMemento>
    {
        public DateTime DueDate { get; private set; }
        public bool Complete { get; private set; }
        public string Name { get; private set; }

        protected Task() {}

        public Task(string taskName, DateTime dueDate, string taskDescription)
        {
            RaiseEvent(new TaskCreated
                           {
                               TaskDescription = taskDescription,
                               TaskId = Guid.NewGuid(),
                               TaskName = taskName,
                               DueDate = dueDate
                           });
        }

        public void MarkComplete()
        {
            //some sort of domain logic may go here if a completed task cannot be completed twice.
            //i'm not caring as it doesn't really affect anything.
            RaiseEvent(new TaskCompleted {TaskId = Id});
        }

        public void Reopen()
        {
            //some sort of domain logic may go here if an incomplete task cannot be reopened.
            //i'm not caring as it doesn't really affect anything.
            RaiseEvent(new TaskReopened {TaskId = Id});
        }

        public void Apply(TaskCreated @event)
        {
            Id = @event.TaskId;
            DueDate = @event.DueDate;
            Name = @event.TaskName;
        }

        public void Apply(TaskCompleted @evemt)
        {
            Complete = true;
        }

        public void Apply(TaskReopened @evemt)
        {
            Complete = false;
        }

        TaskMemento ISnapshotable<TaskMemento>.GetMemento()
        {
            return new TaskMemento {Complete = Complete, Name = Name, DueDate = DueDate};
        }

        void ISnapshotable<TaskMemento>.Hydrate(TaskMemento memento)
        {
            Name = memento.Name;
            Complete = memento.Complete;
            DueDate = memento.DueDate;
        }
    }

    public class TaskMemento
    {
        public DateTime DueDate { get; set; }
        public bool Complete { get; set; }
        public string Name { get; set; }
    }
}
