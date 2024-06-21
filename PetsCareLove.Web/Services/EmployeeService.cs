using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PetsCareLove.Web.Models;
using PetsCareLove.Web.ViewModels;
using System.Text;

namespace PetsCareLove.Web.Services
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public EmployeeService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiUrl = apiSettings.Value.EmployeeUrl;
        }

        #region Employee
        public async Task<List<EmployeeViewModel>> GetAllEmployeesAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Employee");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var employees = JsonConvert.DeserializeObject<List<EmployeeViewModel>>(jsonString);
                return employees;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve employees.");
        }

        public async Task<EmployeeViewModel> GetEmployeeByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Employee/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<EmployeeViewModel>(jsonString);
                return tasks;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve owner.");
        }

        public async Task<HttpResponseMessage> CreateEmployeeAsync(EmployeeViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/api/Employee", content);

            return response;
        }

        public async Task<HttpResponseMessage> EditEmployeeAsync(Guid id, EmployeeViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_apiUrl}/api/Employee/{id}", content);

            return response;
        }

        public async Task<HttpResponseMessage> DeleteEmployeeAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/api/Employee/{id}");

            return response;
        }
        #endregion

        #region Service
        public async Task<List<ServiceViewModel>> GetAllServicesAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Service");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var employees = JsonConvert.DeserializeObject<List<ServiceViewModel>>(jsonString);
                return employees;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve services.");
        }

        public async Task<ServiceViewModel> GetServiceByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Service/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<ServiceViewModel>(jsonString);
                return tasks;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve service.");
        }

        public async Task<HttpResponseMessage> CreateServiceAsync(ServiceViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/api/Service", content);

            return response;
        }

        public async Task<HttpResponseMessage> EditServiceAsync(Guid id, ServiceViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_apiUrl}/api/Service/{id}", content);

            return response;
        }

        public async Task<HttpResponseMessage> DeleteServiceAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/api/Service/{id}");

            return response;
        }
        #endregion
    }
}
