using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PetsCareLove.Web.Models;
using PetsCareLove.Web.ViewModels;
using System.Text;

namespace PetsCareLove.Web.Services
{
    public class PetService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public PetService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiUrl = apiSettings.Value.PetsUrl;
        }

        public async Task<List<PetViewModel>> GetAllPetsAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Pet");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var projects = JsonConvert.DeserializeObject<List<PetViewModel>>(jsonString);
                return projects;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve projects.");
        }

        public async Task<PetViewModel> GetPetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Pet/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<PetViewModel>(jsonString);
                return tasks;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve projects.");
        }

        public async Task<HttpResponseMessage> CreatePetAsync(PetViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/api/Pet", content);

            return response;
        }

        public async Task<HttpResponseMessage> EditPetAsync(Guid id, PetViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_apiUrl}/api/Pet/{id}", content);

            return response;
        }

        public async Task<HttpResponseMessage> DeletePetAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/api/Pet/{id}");

            return response;
        }

        public async Task<List<GenderViewModel>> GetAllGendersAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Gender");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var projects = JsonConvert.DeserializeObject<List<GenderViewModel>>(jsonString);
                return projects;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve projects.");
        }

        public async Task<List<TypeAnimalViewModel>> GetAllTypeAnimalAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/TypeAnimal");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var projects = JsonConvert.DeserializeObject<List<TypeAnimalViewModel>>(jsonString);
                return projects;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve projects.");
        }

        public async Task<List<BreedViewModel>> GetAllAnimalRaceAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Breed");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var projects = JsonConvert.DeserializeObject<List<BreedViewModel>>(jsonString);
                return projects;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve projects.");
        }
    }
}
