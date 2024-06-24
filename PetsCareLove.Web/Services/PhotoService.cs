namespace PetsCareLove.Web.Services
{
    public class PhotoService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public PhotoService(HttpClient httpClient, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClient;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<byte[]> GetPhotoAsync(string photoUrl)
        {
            var response = await _httpClient.GetAsync(photoUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            else
            {
                throw new Exception("Failed to retrieve photo");
            }
        }

        public async Task DownloadAndSaveImageAsync(string imageUrl, string localPath)
        {
            if (Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                // Handle URL
                using (var client = _httpClientFactory.CreateClient())
                using (HttpResponseMessage response = await client.GetAsync(imageUrl))
                {
                    response.EnsureSuccessStatusCode();
                    using (Stream stream = await response.Content.ReadAsStreamAsync())
                    using (FileStream fileStream = new FileStream(localPath, FileMode.Create, FileAccess.Write))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
            }
            else if (File.Exists(imageUrl))
            {
                // Handle local file path
                File.Copy(imageUrl, localPath, true);
            }
            else
            {
                throw new Exception("Invalid photo path");
            }
        }
    }
}
