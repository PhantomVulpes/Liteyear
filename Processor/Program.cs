using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Vulpes.Liteyear.Processor;

static string[] RegistrationNamespaces() => [
    "Vulpes.Liteyear.Domain",
    "Vulpes.Liteyear.External",
    "Vulpes.Liteyear.Processor",
];

var builder = Host.CreateApplicationBuilder(args);

builder.Services
.AddProcessorServices(RegistrationNamespaces())
.AddHostedService<ProcessorService>()
;

await builder.Build().RunAsync();