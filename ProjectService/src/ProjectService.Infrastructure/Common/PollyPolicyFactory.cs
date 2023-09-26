using Polly;
using Polly.Extensions.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectService.Infrastructure.Common
{
    public static class PollyPolicyFactory
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg =>
                {
                    if (msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return true;

                    if (!msg.Content.IsJson()) // Ensure the content is JSON
                        return false;

                    var content = msg.Content.ReadAsStringAsync().Result;
                    return content.Contains("\"error\":");
                })
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
    public static class HttpContentExtensions
    {
        public static bool IsJson(this HttpContent content)
        {
            var mediaType = content?.Headers?.ContentType?.MediaType;
            return string.Equals(mediaType, "application/json", StringComparison.OrdinalIgnoreCase);
        }
    }
}
