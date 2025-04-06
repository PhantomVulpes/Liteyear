namespace Vulpes.Liteyear.Domain.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<TMessage>(LiteyearQueue queue, TMessage message) where TMessage : LiteyearMessage;
}
