using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using Vulpes.Liteyear.Domain.Logging;
using Vulpes.Liteyear.Domain.Messaging;

namespace Vulpes.Liteyear.External.Rabbit;

public class RabbitMqSubscriber : IMessageSubscriber
{
    private readonly IRabbitMqConnectionManager connectionManager;
    private readonly IHostApplicationLifetime lifetime;
    private readonly ILogger<RabbitMqSubscriber> logger;
    private readonly Lazy<Task<IChannel>> subscribeChannel;

    public RabbitMqSubscriber(IRabbitMqConnectionManager connectionManager, IHostApplicationLifetime lifetime, ILogger<RabbitMqSubscriber> logger)
    {
        this.connectionManager = connectionManager;
        this.lifetime = lifetime;
        this.logger = logger;

        subscribeChannel = new Lazy<Task<IChannel>>(CreateSubscribeChannel);
    }

    public async Task SubscribeAsync(LiteyearQueue queue, Func<string, Task> consumer)
    {
        var channel = await subscribeChannel.Value;
        await connectionManager.EnsureQueue(channel, queue);

        var basicConsumer = new AsyncEventingBasicConsumer(channel);

        basicConsumer.ShutdownAsync += (sender, args) =>
        {
            logger.LogError($"{LogTags.ConsumerFatal} Consumer for Queue '{queue.Name}' was shut down. Stopping application.");
            lifetime.StopApplication();

            return Task.CompletedTask;
        };

        basicConsumer.UnregisteredAsync += (sender, args) =>
        {
            logger.LogWarning($"{LogTags.ConsumerFatal} Consumer for Queue '{queue.Name}' was unregistered. Stopping application.");
            lifetime.StopApplication();

            return Task.CompletedTask;
        };

        basicConsumer.ReceivedAsync += async (model, args) =>
        {
            var messageRaw = string.Empty;
            try
            {
                var bytes = args.Body.ToArray();
                messageRaw = System.Text.Encoding.UTF8.GetString(bytes);

                await consumer(messageRaw);

                await channel.BasicAckAsync(args.DeliveryTag, false);
                logger.LogInformation($"{LogTags.MessageSuccess} Message from Queue '{queue.Name}' processed successfully.");
            }
            catch (Exception ex)
            {
                await channel.BasicNackAsync(args.DeliveryTag, false, false);
                logger.LogError(ex, $"{LogTags.MessageFailure} Failed to process message from Queue '{queue.Name}': {ex.Message}.");
            }

            await channel.BasicConsumeAsync(queue.Name, false, basicConsumer);
            logger.LogInformation($"{LogTags.ConsumerStart} Consumer for Queue '{queue.Name}' is ready to receive messages.");
        };
    }

    private async Task<IChannel> CreateSubscribeChannel()
    {
        var connection = await connectionManager.SubscribeConnection;
        var channel = await connection.CreateChannelAsync();
        return channel;
    }

    public async ValueTask DisposeAsync()
    {
        if (subscribeChannel.IsValueCreated)
        {
            (await subscribeChannel.Value).Dispose();
        }
    }
}
