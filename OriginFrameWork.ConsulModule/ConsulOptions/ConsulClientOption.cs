namespace OriginFrameWork.ConsulModule.ConsulOptions
{
    public class ConsulClientOption
    {
        // 公共属性 ServiceGroup，用于存储服务的分组名称
        public string ServiceGroup { get; set; }
        // 公共属性 IP，用于存储服务的IP地址
        public string IP { get; set; }
        // 公共属性 Port，用于存储服务的端口号
        public int Port { get; set; }
        // 公共属性 HealthUrl，用于存储服务的健康检查URL
        public string HealthUrl { get; set; }
        //健康检查间隔时间
        public int Interval { get; set; }
        //健康检查超时时间
        public int Timeout { get; set; }
        //健康检查超时后销毁时间
        public int DeregisterCriticalServiceAfter { get; set; }
        // 公共属性 HttpScheme，用于存储服务的HTTP协议方案（如http或https）
        public bool IsHttps { get; set; }
        // 轮询种类分为 RoundRobin（轮询）/ Weight（权重）/ Random（随机）
        public LoadBalancerOption LoadBalancerOption { get; set; }
    }
    public class LoadBalancerOption
    {
        public string Type { get; set; }
        public int Weight { get; set; }
    }
}
