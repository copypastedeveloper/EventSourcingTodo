using System;

namespace Example.Web.Models
{
    public class TaskEntryViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}