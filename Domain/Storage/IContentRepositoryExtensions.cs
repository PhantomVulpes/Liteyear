using Vulpes.Liteyear.Domain.Configuration;

namespace Vulpes.Liteyear.Domain.Storage;

public static class IContentRepositoryExtensions
{
    public static async Task<byte[]> GetDocumentAsync(this IContentRepository contentRepository, string key) => await contentRepository.GetDocumentAsync(ApplicationConfiguration.DuraluminBucket, key);
}