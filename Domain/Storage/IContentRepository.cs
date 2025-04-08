namespace Vulpes.Liteyear.Domain.Storage;

public interface IContentRepository
{
    Task<string> StoreDocumentAsync(byte[] content, Guid executionFlowKey, string storageEvent);
    Task<byte[]> GetDocumentAsync(string bucket, string key);
}