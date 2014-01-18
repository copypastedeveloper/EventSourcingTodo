using MassTransit;
using Example.Todo.Domain.Events;
using Example.Web.ViewModels;

namespace Example.Web.Denormalizer
{
    public class TaskCreatedConsumer : Consumes<TaskCreated>.All
    {
        readonly IRepository _repository;

        public TaskCreatedConsumer(IRepository repository)
        {
            _repository = repository;
        }

        public void Consume(TaskCreated message)
        {
            var taskViewModel = new TaskViewModel { Description = message.TaskDescription, Id = message.TaskId, Name = message.TaskName, DueDate = message.DueDate };
            _repository.Insert(taskViewModel);

            var taskListViewModel = new TaskListViewModel { Id = message.TaskId, Name = message.TaskName, DueDate = message.DueDate };
            _repository.Insert(taskListViewModel);
        }
    }
}