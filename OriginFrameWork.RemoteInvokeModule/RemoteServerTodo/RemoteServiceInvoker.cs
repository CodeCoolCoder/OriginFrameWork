

using OriginFrameWork.RemoteInvokeModule.RemoteServiceDiscovery;
using System.Text;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OriginFrameWork.RemoteInvokeModule.RemoteServerTodo
{
    /// <summary>
    /// 基于HTTP协议的远程服务调用器
    /// </summary>
    public class RemoteServiceInvoker : IRemoteServiceInvoker
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceDiscovery _serviceDiscovery;

        public RemoteServiceInvoker(HttpClient httpClient, IServiceDiscovery serviceDiscovery)
        {
            _httpClient = httpClient;
            _serviceDiscovery = serviceDiscovery;
        }

        /// <summary>
        /// 执行远程调用
        /// </summary>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="context">调用上下文</param>
        /// <returns>远程调用结果</returns>
        public async Task<TResult> InvokeAsync<TResult>(RemoteServiceInvocationContext context)
        {
            // 解析服务地址
            var serviceAddressDic = await _serviceDiscovery.ResolveServiceAddressAsync(context.ServiceName);
            var address = serviceAddressDic["BaseUrl"];
            var prefix = serviceAddressDic["Prefix"];

            var requestMessage = CreateRequestMessage(context, address, prefix);

            // 发送请求并处理响应
            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResult>(content);
        }


        private HttpRequestMessage CreateRequestMessage(RemoteServiceInvocationContext context, string address, string prefix)
        {
            var requestMessage = new HttpRequestMessage();
            if (context.RequestType == "GET")
            {
                var splic = "";
                var newsplic = "";
                foreach (var item in context.Parameters)
                {
                    var type = context.TypeParameters[item.Key];
                    splic += item.Key + "=" + Convert.ChangeType(item.Value, type) + "&";
                }
                if (splic != "")
                {
                    newsplic = splic.Substring(0, splic.Length - 1);
                    requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{address}/{prefix}/{context.ServiceName}/{context.MethodName}?{newsplic}");
                }
                else
                {
                    requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{address}/{prefix}/{context.ServiceName}/{context.MethodName}");
                }

            }
            else
            {
                //post请求可能既有对象，又有字符串之类的参数
                var jsonStr = "";
                var combineStr = "";
                var url = "";
                foreach (var item in context.PostData.Keys)
                {
                    if (item == typeof(string) || item == typeof(int)
                        || item == typeof(float) || item == typeof(double) || item == typeof(decimal) || item == typeof(bool))
                    {
                        foreach (var str in context.Parameters)
                        {
                            var type = context.TypeParameters[str.Key];
                            combineStr += str.Key + "=" + Convert.ChangeType(str.Value, type) + "&";
                        }
                    }
                    else
                    {
                        jsonStr = context.PostData[item];
                    }
                }
                var newcombineStr = combineStr == "" ? "" : combineStr.Substring(0, combineStr.Length - 1);
                if (newcombineStr == "")
                {
                    url = $"{address}/{prefix}/{context.ServiceName}/{context.MethodName}";
                }
                else
                {
                    url = $"{address}/{prefix}/{context.ServiceName}/{context.MethodName}?{newcombineStr}";
                }
                //post请求
                requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(context.PostData.Values.FirstOrDefault(), Encoding.UTF8, "application/json")
                };
            }
            return requestMessage;
        }
    }
}