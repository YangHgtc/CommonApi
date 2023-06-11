using CommonApi.DTO.Responses;

namespace CommonApi.Business.IServices
{
    public interface IWeatherService
    {
        IEnumerable<WeatherResponse> GetWeather();
    }
}
