using System.Collections.Generic;
using MassTransit;
using MedArchon.Todo.Domain.Events;
using MedArchon.Web.ViewModels;

namespace MedArchon.Web.Denormalizer
{
    public class TaskCompletedConsumer : Consumes<TaskCompleted>.All
    {
        readonly IRepository _repository;

        public TaskCompletedConsumer(IRepository repository)
        {
            _repository = repository;
        }

        public void Consume(TaskCompleted message)
        {
            _repository.Update(message.TaskId, new SetOperationList<TaskViewModel> {{x => x.Complete, true}});

            _repository.Update(message.TaskId, new SetOperationList<TaskListViewModel> {{x => x.Completed, true}});
        }
    }
}