using System;

namespace MedArchon.Web.ViewModels
{
    public class TaskListViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }
    }
}