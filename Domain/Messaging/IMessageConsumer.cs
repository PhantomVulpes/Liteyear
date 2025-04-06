namespace Vulpes.Liteyear.Domain.Messaging;

public interface IMessageConsumer
{
    LiteyearQueue Queue { get; }
    Task ConsumeMessageAsync(string message);
}
