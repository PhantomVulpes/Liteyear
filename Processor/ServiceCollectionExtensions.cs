using Microsoft.Extensions.DependencyInjection;
using Vulpes.Liteyear.Domain.Messaging;
using Vulpes.Liteyear.External.Rabbit;
using Vulpes.Liteyear.Domain;

namespace Vulpes.Liteyear.Processor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProcessorServices(this IServiceCollection services, string[] namespaces) => services
        .ConfigureEnumerableService<IMessageConsumer>(namespaces)
        .AddTransient<IRabbitMqConnectionManager, RabbitMqConnectionManager>()
        .AddTransient<IMessageSubscriber, RabbitMqSubscriber>()
        ;
}