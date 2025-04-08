namespace Vulpes.Liteyear.Domain.Storage;

public interface IContentRepository
{
    Task StoreDocumentAsync(byte[] content, string key);
    Task<byte[]> GetDocumentAsync(string key);
    Task<byte[]> GetExternalDocumentAsync(string bucket, string key);   // TODO: This should be the get document method, other should be an extension.
}