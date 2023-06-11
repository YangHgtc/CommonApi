namespace CommonApi.Api.Controllers;

using CommonApi.Api.Common;
using CommonApi.Business.IServices;
using CommonApi.DTO.Requests;
using CommonApi.DTO.Responses;
using Microsoft.AspNetCore.Mvc;

public class WeatherForecastController : BaseController
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherService _weatherService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
    {
        _logger = logger;
        _weatherService = weatherService;
    }

    [HttpGet]
    public IEnumerable<WeatherResponse> GetWeatherForecast()
    {
        _logger.LogDebug("");
        return _weatherService.GetWeather();
    }

    [HttpPost]
    public bool PostWeatherForecast(WeatherRequest request)
    {
        return true;
    }
}
