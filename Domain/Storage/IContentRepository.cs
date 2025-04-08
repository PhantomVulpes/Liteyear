namespace Vulpes.Liteyear.Domain.Storage;

public interface IContentRepository
{
    Task StoreDocumentAsync(byte[] content, string key);
    Task<byte[]> GetDocumentAsync(string bucket, string key);
}