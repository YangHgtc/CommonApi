using Microsoft.AspNetCore.Authorization;

namespace CommonApi.Api.Controllers;

using CommonApi.Business.IServices;
using CommonApi.Common.Common;
using CommonApi.DTO.Requests;
using CommonApi.DTO.Responses;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// 
/// </summary>
[Authorize]
public class WeatherForecastController : ApiControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherService _weatherService;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        IWeatherService weatherService,
        IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _logger = logger;
        _weatherService = weatherService;
    }

    /// <summary>
    /// 获取天气
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ResponseResult<IEnumerable<WeatherResponse>> GetWeatherForecast()
    {
        _logger.LogDebug("");
        var res = _weatherService.GetWeather();
        return Success(res);
    }

    /// <summary>
    /// 请求天气
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ResponseResult<bool>> PostWeatherForecast(WeatherRequest request)
    {
        await ValidateRequest(request);
        return Success(true);
    }
}
