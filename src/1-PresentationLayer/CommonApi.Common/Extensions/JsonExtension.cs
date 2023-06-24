using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using CommonApi.Util;
using Microsoft.Extensions.DependencyInjection;

namespace CommonApi.Common.Extensions;

public static class JsonExtension
{
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
