using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CommonApi.Common.Common;

/// <summary>
/// 验证基类
/// </summary>
public abstract class ValidateControllerBase : ApiControllerBase
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    ///
    /// </summary>
    /// <param name="serviceProvider"></param>
    protected ValidateControllerBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 验证请求是否符合规则
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="request"></param>
    /// <returns></returns>
    protected async Task ValidateRequest<T>(T request)
    {
        var validator = _serviceProvider.GetService<IValidator<T>>()!;
        await validator.ValidateAndThrowAsync(request);
    }
}
