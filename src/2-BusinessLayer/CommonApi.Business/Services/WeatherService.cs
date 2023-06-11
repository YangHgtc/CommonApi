using CommonApi.Business.IServices;
using CommonApi.DTO.Responses;
using CommonApi.Repository.IRepositories;

namespace CommonApi.Business.Services
{
    public sealed class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository _weatherRepository;

        public WeatherService(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }

        public IEnumerable<WeatherResponse> GetWeather()
        {
            var summaries = _weatherRepository.GetSummaries();
            return Enumerable.Range(1, 5).Select(index => new WeatherResponse
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            })
            .ToArray();
        }
    }
}
