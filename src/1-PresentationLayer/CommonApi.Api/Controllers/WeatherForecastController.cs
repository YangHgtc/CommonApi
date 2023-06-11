namespace CommonApi.Api.Controllers;

using CommonApi.Api.Common;
using CommonApi.Business.IServices;
using CommonApi.DTO.Requests;
using CommonApi.DTO.Responses;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// 
/// </summary>
public class WeatherForecastController : BaseController
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherService _weatherService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
    {
        _logger = logger;
        _weatherService = weatherService;
    }

    /// <summary>
    /// 获取天气
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<WeatherResponse> GetWeatherForecast()
    {
        _logger.LogDebug("");
        return _weatherService.GetWeather();
    }

    /// <summary>
    /// 请求天气
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public bool PostWeatherForecast(WeatherRequest request)
    {
        return true;
    }
}
