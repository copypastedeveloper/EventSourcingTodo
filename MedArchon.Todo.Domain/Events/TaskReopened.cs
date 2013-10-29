using System;

namespace MedArchon.Todo.Domain.Events
{
    public class TaskReopened
    {
        public Guid TaskId { get; set; }
    }
}