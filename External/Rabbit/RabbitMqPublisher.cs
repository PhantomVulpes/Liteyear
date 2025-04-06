using RabbitMQ.Client;
using System.Text.Json;
using Vulpes.Liteyear.Domain.Configuration;
using Vulpes.Liteyear.Domain.Messaging;

namespace Vulpes.Liteyear.External.Rabbit;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly IRabbitMqConnectionManager connectionManager;
    private readonly Lazy<Task<IChannel>> publishChannel;

    public RabbitMqPublisher(IRabbitMqConnectionManager connectionManager)
    {
        this.connectionManager = connectionManager;
        publishChannel = new Lazy<Task<IChannel>>(CreatePublishChannel);
    }

    public async Task PublishAsync<TMessage>(LiteyearQueue queue, TMessage message)
        where TMessage : LiteyearMessage
    {
        var channel = await publishChannel.Value;
        await connectionManager.EnsureQueue(channel, queue);

        var messageBody = JsonSerializer.SerializeToUtf8Bytes(message);
        var routingKey = queue.BuildRoute(message);

        await channel.BasicPublishAsync(ApplicationConfiguration.ExchangeName, routingKey, messageBody);
    }

    private async Task<IChannel> CreatePublishChannel()
    {
        var connection = await connectionManager.PublishConnection;
        var channel = await connection.CreateChannelAsync();
        return channel;
    }

    public async ValueTask DisposeAsync()
    {
        if (publishChannel.IsValueCreated)
        {
            (await publishChannel.Value).Dispose();
        }
    }
}
