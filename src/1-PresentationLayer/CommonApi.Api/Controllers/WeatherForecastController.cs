using CommonApi.Business.IServices;
using CommonApi.Common.Common;
using CommonApi.Common.Filters;
using CommonApi.DTO.Requests;
using CommonApi.DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CommonApi.Api.Controllers;

/// <summary>
/// 天气接口
/// </summary>
//[Authorize]
public class WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService) : ApiControllerBase
{
    /// <summary>
    /// 获取天气
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ResponseResult<bool>), 400)]
    [ProducesResponseType(500)]
    public ResponseResult<IEnumerable<WeatherResponse>> GetWeatherForecast()
    {
        logger.LogDebug("");
        var res = weatherService.GetWeather();
        return Success(res);
    }

    /// <summary>
    /// 请求天气
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidationFilter<WeatherRequest>]
    public ResponseResult<bool> PostWeatherForecast(WeatherRequest request)
    {
        return Success(true);
    }

    [HttpGet]
    public IResult GetNavite()
    {
        return Results.Ok("412121312");
    }
}
