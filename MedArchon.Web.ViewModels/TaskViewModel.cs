using System;

namespace MedArchon.Web.ViewModels
{
    public class TaskViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Name { get; set; }
        public bool Complete { get; set; }
    }
}