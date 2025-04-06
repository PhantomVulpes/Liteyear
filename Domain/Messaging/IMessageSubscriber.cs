namespace Vulpes.Liteyear.Domain.Messaging;

public interface IMessageSubscriber
{
    Task SubscribeAsync(LiteyearQueue queue, Func<string, Task> consumer);
}
