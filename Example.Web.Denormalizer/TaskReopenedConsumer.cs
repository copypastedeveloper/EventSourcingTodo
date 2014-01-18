using MassTransit;
using Example.Todo.Domain.Events;
using Example.Web.ViewModels;

namespace Example.Web.Denormalizer
{
    public class TaskReopenedConsumer : Consumes<TaskReopened>.All
    {
        readonly IRepository _repository;

        public TaskReopenedConsumer(IRepository repository)
        {
            _repository = repository;
        }

        public void Consume(TaskReopened message)
        {
            _repository.Update(message.TaskId, new SetOperationList<TaskViewModel> {{x => x.Complete, false}});

            _repository.Update(message.TaskId, new SetOperationList<TaskListViewModel> {{x => x.Completed, false}});
        }
    }
}