using System.Net.Http.Headers;
using Vulpes.Liteyear.Domain.Configuration;

namespace Vulpes.Liteyear.External.Duralumin;

public class DuraluminClientFactory : IDuraluminClientFactory
{
    private Lazy<DuraluminClient> client;

    public DuraluminClientFactory()
    {
        client = new Lazy<DuraluminClient>(Builder);
    }

    public DuraluminClient BuildClient() => client.Value;

    private DuraluminClient Builder()
    {
        // Create a custom HttpClientHandler to ignore SSL certificate errors.
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

        // Create a new HttpClient with the custom handler.
        var client = new HttpClient(handler);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));

        return new DuraluminClient(PrivateAppSettings.DuraluminEndpoint, client);
    }
}