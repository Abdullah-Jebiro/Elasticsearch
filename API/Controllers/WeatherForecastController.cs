using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ElasticClientsManager elasticClientsManager;

        public WeatherForecastController(ILogger<WeatherForecastController> logger , ElasticClientsManager elasticClientsManager)
        {
            _logger = logger;
            this.elasticClientsManager = elasticClientsManager;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            await elasticClientsManager.ExecuteSearch();
            await elasticClientsManager.ExecuteAggregation("report-submission-summary", "ProviderId");
            await elasticClientsManager.ExecuteAggregation("report-submission-summary", "PlanName");
            await elasticClientsManager.ExecuteAggregation("report-submission-summary", "EncounterPatientId");
            await elasticClientsManager.ExecuteAggregation("report-submission-summary", "PlanName");
            await elasticClientsManager.ExecuteAggregation("report-submission-summary", "PlanName");
            await elasticClientsManager.ExecuteSearch();
            _logger.LogInformation("Fetching weather forecast data.");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}