namespace OriginFrameWork.RemoteInvokeModule.RemoteServer
{
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
