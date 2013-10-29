using System;

namespace MedArchon.Common.Commands
{
    public class CreateTaskCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}