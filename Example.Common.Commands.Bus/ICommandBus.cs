namespace Example.Common.Commands.Bus
{
    public interface ICommandBus
    {
        CommandResponse Send(object command);
    }
}