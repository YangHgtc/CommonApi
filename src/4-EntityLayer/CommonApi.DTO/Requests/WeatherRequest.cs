namespace CommonApi.DTO.Requests;

/// <summary>
/// 
/// </summary>
/// <param name="MaxTemp"></param>
public sealed record WeatherRequest(int MaxTemp, string MinTemp);
