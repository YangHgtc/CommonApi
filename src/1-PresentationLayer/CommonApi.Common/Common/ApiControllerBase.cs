using Microsoft.AspNetCore.Mvc;

namespace CommonApi.Common.Common;

/// <summary>
/// api基类
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>
    /// 失败时返回
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <param name="message"> </param>
    /// <returns> </returns>
    protected ResponseResult<T> Fail<T>(string message)
    {
        return new ResponseResult<T> { Code = (int)ResponseStatusCode.Fail, Message = message };
    }

    /// <summary>
    /// 成功时返回
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <param name="result"> </param>
    /// <param name="message"> </param>
    /// <returns> </returns>
    protected ResponseResult<T> Success<T>(T result, string message = "")
    {
        return new ResponseResult<T> { Code = (int)ResponseStatusCode.Success, Message = message, Result = result };
    }
}
