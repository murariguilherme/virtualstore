using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VS.WebApp.MVC.Extensions;

namespace VS.WebApp.MVC.Services
{
    public abstract class BaseService
    {
        protected async Task<T> DeserializeObjectAsync<T>(HttpResponseMessage httpResponse)
        {
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return await JsonSerializer.DeserializeAsync<T>(await httpResponse.Content.ReadAsStreamAsync(), options);
        }

        protected StringContent GetContent(object data)
        {
            return new StringContent
                (
                    JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"
                );
        }
        public bool CanResolveErrorMessages(HttpResponseMessage message)
        {
            switch ((int)message.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(message.StatusCode);
                case 400:
                    return false;
            }

            message.EnsureSuccessStatusCode();
            return true;
        }
    }
}
