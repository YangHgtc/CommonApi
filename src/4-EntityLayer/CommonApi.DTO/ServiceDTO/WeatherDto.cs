namespace CommonApi.DTO.ServiceDTO;

public sealed class WeatherDto
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF { get; set; }

    public string? Summary { get; set; }
}
