using System;
using System.Linq;
using Given.Common;
using Given.NUnit;
using Example.Todo.Domain.Events;

namespace Example.Todo.Domain.UnitTests
{
    public class reopening_a_completed_task : Scenario
    {
        static Task _task;

        given a_completed_task = () => _task = new Task("test", DateTime.Now.AddDays(2), "test");

        when reopening_the_task = () => _task.Reopen();

        [then]
        public void the_task_completed_event_should_be_raised()
        {
            _task.UncommittedEvents().Last().ShouldBeOfType<TaskReopened>();
        }

        [then]
        public void the_task_should_be_completed()
        {
            _task.Complete.ShouldBeFalse();
        }
    }
}