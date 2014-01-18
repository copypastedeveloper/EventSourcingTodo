using System;
using System.Linq;
using Given.Common;
using Given.NUnit;
using Example.Todo.Domain.Events;

namespace Example.Todo.Domain.UnitTests
{
    public class creating_a_task : Scenario
    {
        static string _description;
        static string _name;
        static DateTime _dueDate;
        static Task _task;

        given valid_task_creation_data = () =>
        {
            _dueDate = DateTime.Now.AddDays(2);
            _name = "test";
            _description = "test";
        };
        
        when adding_a_task_to_the_domain = () => _task = new Task(_name, _dueDate, _description);

        [then]
        public void the_task_created_event_should_be_raised()
        {
            _task.UncommittedEvents().First().ShouldBeOfType<TaskCreated>();
        }

        [then]
        public void the_task_should_reflect_the_creation_data()
        {
            _task.DueDate.ShouldEqual(_dueDate);
        }

        [then]
        public void the_id_should_be_set()
        {
            _task.Id.ShouldNotEqual(new Guid());
        }
    }
}
