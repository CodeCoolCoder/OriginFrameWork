using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OriginFrameWork.Core.Configuration;

namespace OriginFrameWork.Core.Utils;

public static class TokenHelper
{
    public static string CreateToken(JWTTokenModel jWTToken)
    {
        /// <summary>
        /// claims相当于一张入场卷，每一个claim相当于入场卷内的一条信息，所有信息组合起来决定你是否可以入场
        /// </summary>
        /// <value></value>
        var claims = new[] {
            new Claim("Id",jWTToken.Id.ToString()) ,
            new Claim("UserNo",jWTToken.UserNo) ,
            new Claim("UserName",jWTToken.UserName) ,
        };
        //利用SymmetricSecurityKey生成密钥
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTToken.Security));
        /// <summary>
        /// 表示用于生成数字签名的加密密钥和安全算法。
        /// </summary>
        /// <returns>SecurityKey key--包含用于生成数字签名的加密密钥的安全密钥。</returns>
        /// <returns>SecurityAlgorithms 一个 URI，表示用于生成数字签名的加密算法。</returns>
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //获得token的实例
        var token = new JwtSecurityToken(
            issuer: jWTToken.Issuer,
            audience: jWTToken.Audience,
            expires: DateTime.Now.AddMinutes(jWTToken.Expires),
            signingCredentials: creds,
            claims: claims);
        //将此实例处理的类型标记序列化为 XML。
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        return accessToken;
    }
}
