using RabbitMQ.Client;
using Vulpes.Liteyear.Domain.Messaging;

namespace Vulpes.Liteyear.External.Rabbit;

public interface IRabbitMqConnectionManager
{
    Task<IConnection> PublishConnection { get; }
    Task<IConnection> SubscribeConnection { get; }
    Task EnsureQueue(IChannel model, LiteyearQueue queue);
}