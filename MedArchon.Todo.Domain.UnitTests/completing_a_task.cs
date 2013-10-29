using System;
using System.Linq;
using Given.Common;
using Given.NUnit;
using MedArchon.Todo.Domain.Events;

namespace MedArchon.Todo.Domain.UnitTests
{
    public class completing_a_task : Scenario
    {
        static string _description;
        static string _name;
        static DateTime _dueDate;
        static Task _task;

        given a_task = () => _task = new Task("test", DateTime.Now.AddDays(2), "test");

        when marking_a_task_as_complete = () => _task.MarkComplete(); 

        [then]
        public void the_task_completed_event_should_be_raised()
        {
            _task.UncommittedEvents().Last().ShouldBeOfType<TaskCompleted>();
        }

        [then]
        public void the_task_should_be_completed()
        {
            _task.Complete.ShouldBeTrue();
        }
    }
}