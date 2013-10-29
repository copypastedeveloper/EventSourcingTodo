using System;

namespace MedArchon.Todo.Domain.Events
{
    public class TaskCompleted
    {
        public Guid TaskId { get; set; }
    }
}