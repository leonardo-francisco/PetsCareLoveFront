using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PetsCareLove.Web.Dtos;
using PetsCareLove.Web.Models;
using PetsCareLove.Web.ViewModels;
using System.Text;

namespace PetsCareLove.Web.Services
{
	public class AuthService
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiUrl;

		public AuthService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
		{
			_httpClient = httpClient;
			_apiUrl = apiSettings.Value.AuthUrl;
		}

		#region Authentication
		public async Task<HttpResponseMessage> LoginAsync(LoginDto model)
		{

			var jsonContent = JsonConvert.SerializeObject(model);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync($"{_apiUrl}/api/Authentication/login", content);

			return response;
		}

		public async Task<HttpResponseMessage> RegisterAsync(UserDto model)
		{

			var jsonContent = JsonConvert.SerializeObject(model);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync($"{_apiUrl}/api/Authentication/registrar", content);

			return response;
		}

        public async Task<HttpResponseMessage> RecoveryPasswordAsync(LoginDto model)
        {

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/api/Authentication/recovery-password", content);

            return response;
        }
        #endregion

        #region Permissions

        #endregion

        #region Roles
        public async Task<List<RoleViewModel>> GetAllRolesAsync()
		{
			var response = await _httpClient.GetAsync($"{_apiUrl}/api/Roles");

			if (response.IsSuccessStatusCode)
			{
				var jsonString = await response.Content.ReadAsStringAsync();
				var roles = JsonConvert.DeserializeObject<List<RoleViewModel>>(jsonString);
				return roles;
			}

			// Trate erros de acordo
			throw new Exception("Failed to retrieve roles.");
		}

		public async Task<RoleViewModel> GetRolesByIdAsync(Guid id)
		{
			var response = await _httpClient.GetAsync($"{_apiUrl}/api/Roles/{id}");

			if (response.IsSuccessStatusCode)
			{
				var jsonString = await response.Content.ReadAsStringAsync();
				var tasks = JsonConvert.DeserializeObject<RoleViewModel>(jsonString);
				return tasks;
			}

			// Trate erros de acordo
			throw new Exception("Failed to retrieve roles.");
		}

		public async Task<HttpResponseMessage> CreateRolesAsync(RoleViewModel model)
		{

			var jsonContent = JsonConvert.SerializeObject(model);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync($"{_apiUrl}/api/Roles", content);

			return response;
		}

		public async Task<HttpResponseMessage> EditRolesAsync(Guid id, RoleViewModel model)
		{

			var jsonContent = JsonConvert.SerializeObject(model);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync($"{_apiUrl}/api/Roles/{id}", content);

			return response;
		}

		public async Task<HttpResponseMessage> DeleteRolesAsync(Guid id)
		{
			var response = await _httpClient.DeleteAsync($"{_apiUrl}/api/Roles/{id}");

			return response;
		}
		#endregion


	}
}
