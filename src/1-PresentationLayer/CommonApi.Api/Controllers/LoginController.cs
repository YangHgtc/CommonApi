using System.Security.Claims;
using CommonApi.Common.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CommonApi.Api.Controllers;

/// <summary>
/// 登录接口
/// </summary>
public sealed class LoginController : ApiControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly IOptions<JwtOptions> _options;

    /// <summary>
    /// </summary>
    /// <param name="jwtService"> </param>
    /// <param name="options"> </param>
    public LoginController(IJwtService jwtService, IOptions<JwtOptions> options)
    {
        _jwtService = jwtService;
        _options = options;
    }

    /// <summary>
    /// 获取token
    /// </summary>
    /// <returns> </returns>
    [AllowAnonymous]
    [HttpGet]
    public ResponseResult<string> GetToken()
    {
        var jwtOptions = _options.Value;
        var claims = new List<Claim> { new(ClaimTypes.Name, "用户1"), new(ClaimTypes.Role, "超级管理员") };
        var token = _jwtService.BuildToken(claims, jwtOptions);
        return Success(token);
    }
}
