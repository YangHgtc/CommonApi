using CommonApi.DTO.Responses;
using CommonApi.DTO.ServiceDTO;
using Mapster;

namespace CommonApi.Mapper.Mapster;

/// <summary>
/// 需要映射的实现IRegister接口
/// </summary>
public sealed class WeatherMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<WeatherDto, WeatherResponse>()
            .Map(dest => dest.TemperatureF, src => 32 + (int)(src.TemperatureC / 0.5556));
    }
}
