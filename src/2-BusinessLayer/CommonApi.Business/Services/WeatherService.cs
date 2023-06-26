using CommonApi.Business.IServices;
using CommonApi.DTO.Responses;
using CommonApi.DTO.ServiceDTO;
using CommonApi.Repository.IRepositories;
using MapsterMapper;

namespace CommonApi.Business.Services;

public sealed class WeatherService : IWeatherService
{
    private readonly IWeatherRepository _weatherRepository;
    private readonly IMapper _mapper;

    public WeatherService(IWeatherRepository weatherRepository,  IMapper mapper)
    {
        _weatherRepository = weatherRepository;
        _mapper = mapper;
    }

    public IEnumerable<WeatherResponse> GetWeather()
    {
        var summaries = _weatherRepository.GetSummaries();
        var arr = Enumerable.Range(1, 5).Select(index => new WeatherDto
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            })
            .ToArray();
        return _mapper.Map<WeatherResponse[]>(arr);
    }
}
