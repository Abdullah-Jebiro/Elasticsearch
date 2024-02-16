using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var indexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower()}" +
    $"-{environment?.ToLower().Replace(".","-")}-{DateTime.UtcNow:yyyy-MM}";

Log.Logger = new LoggerConfiguration()
   .Enrich.FromLogContext()
   .Enrich.WithExceptionDetails()
   .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
       new Uri("http://localhost:9200/"))
       {
           AutoRegisterTemplate = true,
           IndexFormat= indexFormat

   })
   .CreateLogger();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog();

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

app.Run();
