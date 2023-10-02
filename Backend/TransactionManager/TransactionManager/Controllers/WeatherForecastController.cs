using Microsoft.AspNetCore.Mvc;
using TransactionManager.Response;
namespace TransactionManager.Controllers;
using Newtonsoft.Json;
using TransactionManager.Models.DBModels;

[ApiController]
[Route("info")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "info")]
    public IActionResult Get()
    {
        System.Collections.ObjectModel.ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();
        return Ok(timeZones);
    }

}

