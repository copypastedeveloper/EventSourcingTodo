using System;
using MedArchon.Todo.Domain.Common;
using MedArchon.Todo.Domain.Events;

namespace MedArchon.Todo.Domain
{
    public class Task : DomainEntity
    {
        public DateTime DueDate { get; set; }
        public bool Complete { get; set; }

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
        }

        public void Apply(TaskCompleted @evemt)
        {
            Complete = true;
        }

        public void Apply(TaskReopened @evemt)
        {
            Complete = false;
        }
    }
}
