using CommonApi.Business.IServices;
using CommonApi.DTO.Responses;
using CommonApi.DTO.ServiceDTO;
using CommonApi.Repository.IRepositories;
using MapsterMapper;

namespace CommonApi.Business.Services;

public sealed class WeatherService(IWeatherRepository weatherRepository, IMapper mapper) : IWeatherService
{
    public IEnumerable<WeatherResponse> GetWeather()
    {
        var summaries = weatherRepository.GetSummaries();
        var arr = Enumerable.Range(1, 5).Select(index => new WeatherDto
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        })
            .ToArray();
        return mapper.Map<WeatherResponse[]>(arr);
    }
}

