using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PetsCareLove.Web.Models;
using PetsCareLove.Web.ViewModels;
using System.Text;

namespace PetsCareLove.Web.Services
{
    public class OwnerService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public OwnerService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiUrl = apiSettings.Value.OwnersUrl;
        }

        public async Task<List<OwnerViewModel>> GetAllOwnersAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Owner");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var owners = JsonConvert.DeserializeObject<List<OwnerViewModel>>(jsonString);
                return owners;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve owners.");
        }

        public async Task<OwnerViewModel> GetOwnerByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Owner/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<OwnerViewModel>(jsonString);
                return tasks;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve owner.");
        }

        public async Task<HttpResponseMessage> CreateOwnerAsync(OwnerViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/api/Owner", content);

            return response;
        }

        public async Task<HttpResponseMessage> EditOwnerAsync(Guid id,OwnerViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_apiUrl}/api/Owner/{id}", content);

            return response;
        }

        public async Task<HttpResponseMessage> DeleteOwnerAsync(Guid id)
        {          
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/api/Owner/{id}");

            return response;
        }
    }
}
