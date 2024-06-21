using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PetsCareLove.Web.Models;
using PetsCareLove.Web.ViewModels;
using System.Text;

namespace PetsCareLove.Web.Services
{
    public class VeterinarianService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public VeterinarianService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiUrl = apiSettings.Value.VeterinariansUrl;
        }

        #region Veterinarian
        public async Task<List<VeterinarianViewModel>> GetAllVeterinariansAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Veterinarian");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var vets = JsonConvert.DeserializeObject<List<VeterinarianViewModel>>(jsonString);
                return vets;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve veterinarians.");
        }

        public async Task<VeterinarianViewModel> GetVeterinarianByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Veterinarian/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<VeterinarianViewModel>(jsonString);
                return tasks;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve veterinarian.");
        }

        public async Task<HttpResponseMessage> CreateVeterinarianAsync(VeterinarianViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/api/Veterinarian", content);

            return response;
        }

        public async Task<HttpResponseMessage> EditVeterinarianAsync(Guid id, VeterinarianViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_apiUrl}/api/Veterinarian/{id}", content);

            return response;
        }

        public async Task<HttpResponseMessage> DeleteVeterinarianAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/api/Veterinarian/{id}");

            return response;
        }
        #endregion

        #region MedicalRecord
        public async Task<MedicalRecordViewModel> GetMedicalRecordByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/MedicalRecord/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<MedicalRecordViewModel>(jsonString);
                return tasks;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve medical record.");
        }

        public async Task<MedicalRecordViewModel> GetMedicalRecordByPetIdAsync(Guid petId)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/MedicalRecord/Pet/{petId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<MedicalRecordViewModel>(jsonString);
                return tasks;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve medical record.");
        }

        public async Task<MedicalRecordViewModel> GetMedicalRecordByAppointmentIdAsync(Guid appointmentId)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/MedicalRecord/Appointment/{appointmentId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<MedicalRecordViewModel>(jsonString);
                return tasks;
            }

            // Trate erros de acordo
            throw new Exception("Failed to retrieve medical record.");
        }

        public async Task<HttpResponseMessage> CreateMedicalRecordAsync(MedicalRecordViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/api/MedicalRecord", content);

            return response;
        }

        public async Task<HttpResponseMessage> EditMedicalRecordAsync(Guid id, MedicalRecordViewModel model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_apiUrl}/api/MedicalRecord/{id}", content);

            return response;
        }
        #endregion


    }
}
