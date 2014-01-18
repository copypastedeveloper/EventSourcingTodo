using Example.Common.Commands;
using Example.Common.Commands.Bus;
using Example.Todo.Domain;
using Example.Todo.Domain.Common;

namespace Example.CommandHandlers
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
