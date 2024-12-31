namespace OriginFrameWork.RemoteInvokeModule.RemoteAttributes
{   /// <summary>
    /// 标记一个接口为远程服务
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class RemoteServiceAttribute : Attribute
    {
        /// <summary>
        /// 远程服务组名称
        /// </summary>
        public string RemoteServiceGroup { get; set; }
        /// <summary>
        /// 请求方式，默认为POST
        /// </summary>
        public string remoteHttpMethod { get; set; }
        public RemoteServiceAttribute(string remoteServiceGroup, string toHttpMethod = "POST")
        {
            RemoteServiceGroup = remoteServiceGroup;
            remoteHttpMethod = toHttpMethod;
        }
    }
}
