using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Phonebook.Report.API.IoCs;
using Phonebook.Report.Infrastructure.IoCs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddHealthChecks()
    .AddMongoDb(
        mongodbConnectionString: "mongodb://localhost:27017",
        name: "MongoDb Check",
        failureStatus: HealthStatus.Unhealthy | HealthStatus.Healthy,
        tags: new string[] { "mongodb" })
    .AddRabbitMQ(
        rabbitConnectionString: "amqp://guest:guest@localhost:5672",
        name: "RabbitMQ Check",
        failureStatus: HealthStatus.Unhealthy | HealthStatus.Healthy,
        tags: new string[] { "rabbitmq" });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddReportInfraDIs();
builder.ConfigureRabbitMq();
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version"));
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

app.UseHealthChecks("/reportapi-health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
