using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using CommonApi.Util;
using CommonApi.Util.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace CommonApi.Common.Extensions;

/// <summary>
/// 
/// </summary>
public static class JsonExtension
{
    /// <summary>
    /// 添加全局json序列化设置
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddJsonSetting(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(json =>
        {
            json.JsonSerializerOptions.WriteIndented = true;//格式化json
            json.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);//可以序列化所有语言
            json.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;//驼峰大小写
            json.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;//忽略循环引用
            json.JsonSerializerOptions.Converters.Add(new DateTimeConverter());//格式化时间
        });
        return services;
    }
}
