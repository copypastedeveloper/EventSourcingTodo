using MedArchon.Common.Commands;
using MedArchon.Common.Commands.Bus;
using MedArchon.Todo.Domain;
using MedArchon.Todo.Domain.Common;

namespace MedArchon.CommandHandlers
{
    public class CreateTaskCommandHandler : CommandHandler<CreateTaskCommand>
    {
        readonly IEntitySaver _entitySaver;

        public CreateTaskCommandHandler(IEntitySaver entitySaver)
        {
            _entitySaver = entitySaver;
        }

        protected override CommandResponse Handle(CreateTaskCommand command)
        {
            var task = new Task(command.Name, command.DueDate, command.Description);
            _entitySaver.Save(task);
            return new CommandResponse {Success = true};
        }
    }
}
