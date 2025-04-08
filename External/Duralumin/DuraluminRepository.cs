using Microsoft.Extensions.Logging;
using Vulpes.Liteyear.Domain.Configuration;
using Vulpes.Liteyear.Domain.Logging;
using Vulpes.Liteyear.Domain.Storage;

namespace Vulpes.Liteyear.External.Duralumin;

public class DuraluminRepository : IContentRepository
{
    private readonly IDuraluminClientFactory duraluminClientFactory;
    private readonly ILogger<DuraluminRepository> logger;

    public DuraluminRepository(IDuraluminClientFactory duraluminClientFactory, ILogger<DuraluminRepository> logger)
    {
        this.duraluminClientFactory = duraluminClientFactory;
        this.logger = logger;
    }

    public async Task<string> StoreDocumentAsync(byte[] content, Guid executionFlowKey, string storageEvent)
    {
        var client = duraluminClientFactory.BuildClient();
        var builtKey = $"executionFlow-{executionFlowKey}-event-{storageEvent}-content-{Guid.NewGuid()}";

        var storeRequest = new StoreDocumentRequest()
        {
            Bucket = ApplicationConfiguration.DuraluminBucket,
            FileKey = builtKey,
            Bytes = content
        };

        try
        {
            await client.StoreAsync(storeRequest);

            logger.LogInformation($"{LogTags.ContentStorageSuccess} Storing document with key: {builtKey} in bucket: {ApplicationConfiguration.DuraluminBucket}");
            return builtKey;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{LogTags.ContentStorageFailure} Failed to store document with key: {builtKey} in bucket: {ApplicationConfiguration.DuraluminBucket}");
            throw;
        }
    }

    public async Task<byte[]> GetDocumentAsync(string bucket, string key)
    {
        var client = duraluminClientFactory.BuildClient();

        try
        {
            var response = await client.RetrieveAsync(bucket, key);

            logger.LogInformation($"{LogTags.ContentRetrievalSuccess} Retrieving document with key: {key} from bucket: {bucket}");
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{LogTags.ContentRetrievalFailure} Failed to retrieve document with key: {key} from bucket: {bucket}");
            throw;
        }
    }
}
