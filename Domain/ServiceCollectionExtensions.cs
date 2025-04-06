using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Vulpes.Liteyear.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureEnumerableService<TEnumerableCollection>(this IServiceCollection services, params string[] namespaces)
    {
        var targetType = typeof(TEnumerableCollection);

        foreach (var ns in namespaces)
        {
            var assembly = Assembly.Load(ns);

            foreach (var type in assembly.GetTypes().Where(p => !p.IsInterface && !p.IsAbstract && p.IsAssignableTo(targetType)))
            {
                services.AddTransient(targetType, type);
            }
        }

        return services;
    }
}