using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VS.WebApp.MVC.Extensions;
using VS.WebApp.MVC.ViewModels;

namespace VS.WebApp.MVC.Services
{
    public class CatalogService : BaseService, ICatalogService
    {
        private HttpClient _httpClient;
        public CatalogService(HttpClient httpClient, IOptions<AppSettings> appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Value.EndpointCatalog);
        }

        public async  Task<IEnumerable<ProductViewModel>> GetAllProducts()
        {
            var response = await _httpClient.GetAsync("/api/catalog/products");

            CanResolveErrorMessages(response);

            return await DeserializeObjectAsync<IEnumerable<ProductViewModel>>(response);
        }

        public async Task<ProductViewModel> GetProductById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/catalog/product/{id}");

            CanResolveErrorMessages(response);

            return await DeserializeObjectAsync<ProductViewModel>(response);
        }
    }
}
