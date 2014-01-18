using System;

namespace Example.Todo.Domain.Events
{
    public class TaskCreated
    {
        public string TaskDescription { get; set; }
        public Guid TaskId { get; set; }
        public DateTime DueDate { get; set; }
        public string TaskName { get; set; }
    }
}