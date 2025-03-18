namespace OriginFrameWork.RemoteInvokeModule.RemoteAttributes
{
    /// <summary>
    /// 远程调用方法特性（定制化请求方式和是否启用）
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RemoteServiceIndividualAttribute : Attribute
    {
        public bool IsEnable { get; set; }
        public string HttpMethodByMethod { get; set; }
        public RemoteServiceIndividualAttribute(string toHttpMethod, bool isEnble = true)
        {
            HttpMethodByMethod = toHttpMethod;
            IsEnable = isEnble;
        }
    }
}
