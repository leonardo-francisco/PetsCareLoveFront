using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PetsCareLove.Web.Models;
using PetsCareLove.Web.ViewModels;
using System.Text;

namespace PetsCareLove.Web.Services
{
    public class EcommerceService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public EcommerceService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiUrl = apiSettings.Value.EcommerceUrl;
        }

        #region Category
        public async Task<List<CategoryViewModel>> GetAllCategoriesAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Category");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<CategoryViewModel>>(jsonString);
                return categories;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve categories.");
        }

        public async Task<CategoryViewModel> GetCategoryByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Category/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<CategoryViewModel>(jsonString);
                return tasks;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve category.");
        }

        public async Task<HttpResponseMessage> CreateCategoryAsync(CategoryViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/api/Category", content);

            return response;
        }

        public async Task<HttpResponseMessage> EditCategoryAsync(Guid id, CategoryViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_apiUrl}/api/Category/{id}", content);

            return response;
        }

        public async Task<HttpResponseMessage> DeleteCategoryAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/api/Category/{id}");

            return response;
        }
        #endregion

        #region Product
        public async Task<List<ProductViewModel>> GetAllProductsAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Product");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<ProductViewModel>>(jsonString);
                return products;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve products.");
        }

        public async Task<ProductViewModel> GetProductByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Product/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<ProductViewModel>(jsonString);
                return tasks;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve category.");
        }

        public async Task<ProductViewModel> GetProductByCategoryIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Product/Category/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<ProductViewModel>(jsonString);
                return tasks;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve category.");
        }

        public async Task<List<ProductViewModel>> GetProductByCategoryAndPetIdAsync(Guid categoryId, Guid petId)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Product/CategoryAndPet/{categoryId}/{petId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<List<ProductViewModel>>(jsonString);
                return tasks;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve category.");
        }

        public async Task<HttpResponseMessage> CreateProductAsync(ProductViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/api/Product", content);

            return response;
        }

        public async Task<HttpResponseMessage> EditProductAsync(Guid id, ProductViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_apiUrl}/api/Product/{id}", content);

            return response;
        }

        public async Task<HttpResponseMessage> DeleteProductAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/api/Product/{id}");

            return response;
        }
        #endregion
    }
}
