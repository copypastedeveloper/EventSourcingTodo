namespace MedArchon.Common.Commands.Bus
{
    public interface ICommandBus
    {
        CommandResponse Send(object command);
    }
}