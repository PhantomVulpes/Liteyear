using Vulpes.Liteyear.Domain.Storage;
using Vulpes.Liteyear.External.Duralumin;

namespace Vulpes.Liteyear.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddContentRepository(this IServiceCollection services) => services
            .AddSingleton<IDuraluminClientFactory, DuraluminClientFactory>()
            .AddSingleton<IContentRepository, DuraluminRepository>()
            ;
}