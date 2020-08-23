/****************************************************************************
*项目名称：MicroServiceGateway.Common
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Common
*类 名 称：HttpClientWrapper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/20 15:36:35
*描述：
*=====================================================================
*修改时间：2020/8/20 15:36:35
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MicroServiceGateway.Common
{
    /// <summary>
    /// HttpClientWrapper
    /// </summary>
    public class HttpClientWrapper
    {
        HttpClient _client;

        /// <summary>
        /// HttpClient
        /// </summary>
        public HttpClient Client
        {
            get
            {
                return _client;
            }
        }
        /// <summary>
        /// HttpClientWrapper
        /// </summary>
        public HttpClientWrapper()
        {
            _client = new HttpClient();
        }
        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            return Client.SendAsync(request, cancellationToken);
        }
    }
}
