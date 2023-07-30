using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CommonApi.Common.Common;

/// <summary>
///
/// </summary>
public interface IJwtService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    string BuildToken(IEnumerable<Claim> claims, JwtOptions options);
}

/// <summary>
///
/// </summary>
public sealed class JwtService : IJwtService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public string BuildToken(IEnumerable<Claim> claims, JwtOptions options)
    {
        //过期时间
        var timeSpan = TimeSpan.FromSeconds(options.ExpireSeconds);//token过期时间
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));//加密的token密钥
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);//签名证书，其值为securityKey和HmacSha256Signature算法
        var tokenDescriptor = new JwtSecurityToken(options.Issuer, options.Audience, claims, expires: DateTime.Now.Add(timeSpan), signingCredentials: credentials);//表示jwt token的描述信息，其值包括Issuer签发方，Audience接收方，Claims载荷，过期时间和签名证书
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);//使用该方法转换为字符串形式的jwt token返回
    }
}

/// <summary>
///
/// </summary>
public sealed class JwtOptions
{
    /// <summary>
    ///
    /// </summary>
    public const string Position = "JWT";

    /// <summary>
    /// 签发者
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// 接收者
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    /// 密钥
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public int ExpireSeconds { get; set; }
}
