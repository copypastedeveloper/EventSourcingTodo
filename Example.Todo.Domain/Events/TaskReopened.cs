using System;

namespace Example.Todo.Domain.Events
{
    public class TaskReopened
    {
        public Guid TaskId { get; set; }
    }
}