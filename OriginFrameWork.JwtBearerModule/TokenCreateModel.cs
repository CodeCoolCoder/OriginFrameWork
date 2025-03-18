using Microsoft.Extensions.Configuration;

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
                expiresTime = TimeSpan.FromHours(int.Parse(Configuration.GetValue<string>("JwtAuth:Expires"))),
                userId = id
            };
            return JwtCreat.CreateToken(tokenCreateModel);
        }

    }
}
