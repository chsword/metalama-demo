using Microsoft.AspNetCore.Mvc;

namespace AspNetApiDemo.Controllers;

[ApiController]
[Route("[controller]")]
 
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private ILogger<WeatherForecastController> _logger;

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    { 
        _logger.LogInformation("run get");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}