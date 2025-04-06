using RabbitMQ.Client;
using System.Threading.Tasks;
using Vulpes.Liteyear.Domain.Configuration;
using Vulpes.Liteyear.Domain.Messaging;

namespace Vulpes.Liteyear.External.Extensions;

public static class LiteyearQueueExtensions
{
    public static async Task DeclareAsync(this LiteyearQueue queue, IChannel channel)
    {
        await channel.QueueDeclareAsync(queue.DeadLetterName, true, false, false);
        await channel.QueueBindAsync(queue.DeadLetterName, ApplicationConfiguration.DeadLetterExchangeName, queue.RoutingKey);

        await channel.QueueDeclareAsync(queue.Name, true, false, false,
            new Dictionary<string, object?>
            {
                [LiteyearQueue.DeadLetterKey] = ApplicationConfiguration.DeadLetterExchangeName
            });

        await channel.QueueBindAsync(queue.Name, ApplicationConfiguration.ExchangeName, queue.RoutingKey);
    }
}
