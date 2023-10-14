namespace CommonApi.DTO.Responses;

/// <summary>
/// </summary>
public sealed class WeatherResponse
{
    /// <summary>
    /// </summary>
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF { get; set; }

    public string? Summary { get; set; }
}
