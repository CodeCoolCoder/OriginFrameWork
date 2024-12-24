using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginFrameWork.JwtBearerModule
{
    public class TokenCreateModel
    {
        public TokenCreateModel(IConfiguration configuration)
        {
            Configuration = configuration;
          
        }

        public IConfiguration Configuration { get; }
    

        public TokenCreateModel()
        {
            
        }
        public string userId { get; set; }
        public string securityKey { get; set; }
        public string issuer { get; set; }
        public string audience { get; set; }
        public TimeSpan expiresTime { get; set; }
        /// <summary>
        /// 创建token
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetToken(string id)
        {
            TokenCreateModel tokenCreateModel = new TokenCreateModel()
            {
                audience = Configuration.GetValue<string>("JwtAuth:Audience"),
                issuer = Configuration.GetValue<string>("JwtAuth:Issuer"),
                securityKey = Configuration.GetValue<string>("JwtAuth:SecurityKey"),
                expiresTime = TimeSpan.FromHours(2),
                userId = id
            };           
            return JwtCreatService.CreateToken( tokenCreateModel);
        }

    }
}
