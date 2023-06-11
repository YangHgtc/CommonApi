using CommonApi.Repository.IRepositories;

namespace CommonApi.Repository.Repositories
{
    public sealed class WeatherRepository : IWeatherRepository
    {
        public string[] GetSummaries()
        {
            return new[]
                    {
                        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                    };
        }
    }
}
