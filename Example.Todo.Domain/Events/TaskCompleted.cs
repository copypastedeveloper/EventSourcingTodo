using System;

namespace Example.Todo.Domain.Events
{
    public class TaskCompleted
    {
        public Guid TaskId { get; set; }
    }
}