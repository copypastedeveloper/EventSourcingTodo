using Example.Common.Commands;
using Example.Common.Commands.Bus;
using Example.Todo.Domain;
using Example.Todo.Domain.Common;

namespace Example.CommandHandlers
{
    public class ReopenTaskCommandHandler : CommandHandler<ReopenTaskCommand>
    {
        readonly IEntitySaver _entitySaver;
        readonly IEntityBuilder _entityBuilder;

        public ReopenTaskCommandHandler(IEntitySaver entitySaver, IEntityBuilder entityBuilder)
        {
            _entitySaver = entitySaver;
            _entityBuilder = entityBuilder;
        }

        protected override CommandResponse Handle(ReopenTaskCommand command)
        {
            var task = _entityBuilder.GetById<Task>(command.TaskId);
            task.Reopen();
            _entitySaver.Save(task);

            return new CommandResponse { Success = true };
        }
    }
}