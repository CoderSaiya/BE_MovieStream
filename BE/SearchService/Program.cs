using SearchService;
using SearchService.Repositories;
using Elastic.Clients.Elasticsearch;
using SharedLibrary.EventBus;
using SharedLibrary.Events;
using SearchService.Handler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(provider =>
{
    var settings = new ElasticsearchClientSettings(new Uri("http://localhost:9200"))
        .CertificateFingerprint("your-certificate-fingerprint")
        .DefaultIndex("movies");

    return new Elastic.Clients.Elasticsearch.ElasticsearchClient(settings);
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<SearchService.ElasticsearchClient>();
builder.Services.AddScoped<ISearch, SearchRepo>();

// Add RabbitMQ Event Bus
builder.Services.AddSingleton<IEventBus, EventBus>(sp =>
{
    var serviceProvider = sp.GetRequiredService<IServiceProvider>();
    var hostName = builder.Configuration["RabbitMQ:HostName"] ?? "localhost";
    return new EventBus(serviceProvider, hostName);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(() =>
{
    var eventBus = app.Services.GetRequiredService<IEventBus>();
    eventBus.Subscribe<MovieCreatedEvent, MovieCreatedEventHandler>();
});

app.Run();
