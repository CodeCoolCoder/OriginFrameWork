


using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace OriginFrameWork.CoreModule.OriginConfigCore;

public class Appsetting
{
    //public static Dictionary<string, string> Connections { get; set; }
    //获取connection对象
    public static Connection _connection { get; set; }
    /// <summary>
    /// 初始化连接配置文件对象
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void Init(IServiceCollection services, IConfiguration configuration)
    {
        //将配置文件里的值映射到相应的类中
        var res = configuration.GetSection("Connection");
        _connection = configuration.GetSection("Connection").Get<Connection>();
    }
}
