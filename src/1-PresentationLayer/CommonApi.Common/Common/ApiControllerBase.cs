using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CommonApi.Common.Common;

[ApiController]
[Route("api/[controller]/[action]")]
public abstract class ApiControllerBase : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;

    public ApiControllerBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 成功时返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    protected ResponseResult<T> Success<T>(T result, string message = "") => new() { Code = (int)ResponseStatusCode.Success, Message = message, Data = result };

    /// <summary>
    /// 失败时返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    protected ResponseResult<T> Fail<T>(string message) => new() { Code = (int)ResponseStatusCode.Fail, Message = message };

    /// <summary>
    /// 验证请求是否符合规则
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="request"></param>
    /// <returns></returns>
    protected async Task ValidateRequest<T>(T request)
    {
        var type = typeof(IValidator<T>);
        var validator = _serviceProvider.GetService(type)!;
        if (validator is null)
        {
            throw new ArgumentNullException($"请为{typeof(T).FullName}类型编写验证规则,然后重试!");
        }
        await ((IValidator<T>)validator).ValidateAndThrowAsync(request);
    }
}
