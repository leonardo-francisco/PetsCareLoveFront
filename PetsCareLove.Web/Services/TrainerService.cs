using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PetsCareLove.Web.Models;
using PetsCareLove.Web.ViewModels;
using System.Text;

namespace PetsCareLove.Web.Services
{
    public class TrainerService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public TrainerService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiUrl = apiSettings.Value.TrainersUrl;
        }

        #region Trainer
        public async Task<List<TrainerViewModel>> GetAllTrainersAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Trainer");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var vets = JsonConvert.DeserializeObject<List<TrainerViewModel>>(jsonString);
                return vets;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve trainers.");
        }

        public async Task<TrainerViewModel> GetTrainerByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Trainer/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<TrainerViewModel>(jsonString);
                return tasks;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve trainer.");
        }

        public async Task<HttpResponseMessage> CreateTrainerAsync(TrainerViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/api/Trainer", content);

            return response;
        }

        public async Task<HttpResponseMessage> EditTrainerAsync(Guid id, TrainerViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_apiUrl}/api/Trainer/{id}", content);

            return response;
        }

        public async Task<HttpResponseMessage> DeleteTrainerAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/api/Trainer/{id}");

            return response;
        }
        #endregion
    }
}
