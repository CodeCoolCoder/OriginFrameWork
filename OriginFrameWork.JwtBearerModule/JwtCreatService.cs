using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OriginFrameWork.JwtBearerModule
{
    public class JwtCreatService: IJwtCreatService        
    {
        public static string CreateToken(TokenCreateModel tokenCreateModel)
        {
            //用户的id加入claims中识别用户身份
            var claims = new[]
            {
                new Claim("userId",tokenCreateModel.userId)
            };
            //利用SymmetricSecurityKey生成密钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenCreateModel.securityKey));
            /// <summary>
            /// 表示用于生成数字签名的加密密钥和安全算法。
            /// </summary>
            /// <returns>SecurityKey key--包含用于生成数字签名的加密密钥的安全密钥。</returns>
            /// <returns>SecurityAlgorithms 一个 URI，表示用于生成数字签名的加密算法。</returns>
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //获得token的实例
            var token = new JwtSecurityToken(
                issuer: tokenCreateModel.issuer,
                audience: tokenCreateModel.audience,
                expires:DateTime.Now.Add(tokenCreateModel.expiresTime),
                signingCredentials: creds,
                claims: claims);
            //将此实例处理的类型标记序列化为 XML。
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
        }
    }
}
