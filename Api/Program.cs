using Vulpes.Liteyear.Api.Extensions;
using Vulpes.Liteyear.Domain.Messaging;
using Vulpes.Liteyear.Domain.Storage;
using Vulpes.Liteyear.External.Duralumin;
using Vulpes.Liteyear.External.Rabbit;

var builder = WebApplication.CreateBuilder(args);

// Ensure Kestrel configuration is applied
builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Configure(context.Configuration.GetSection("Kestrel"));
});


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TODO: Build extensions.
builder.Services
    .AddContentRepository()
    .AddSingleton<IRabbitMqConnectionManager, RabbitMqConnectionManager>()
    .AddSingleton<IMessagePublisher, RabbitMqPublisher>()
    ;

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}
else
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("swagger/index.html", "API Home");
        c.SwaggerEndpoint("/swagger/v0.1/swagger.json", "Duralumin API");
        c.RoutePrefix = string.Empty; // Set Swagger at the root
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();