namespace OriginFrameWork.ConsulModule.ConsulOptions
{
    /// <summary>
    /// consul服务中心配置
    /// </summary>
    public class ConsulServerOption
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public bool IsHttps { get; set; } = false;
    }
}
