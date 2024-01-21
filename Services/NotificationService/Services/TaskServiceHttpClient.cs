using MongoDB.Entities;
using TaskService;

namespace SearchService.Services
{
    public class AnimalServiceHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public AnimalServiceHttpClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<List<Animal>> GetTasksForNotificationDB()
        {
            return await _httpClient.GetFromJsonAsync<List<Animal>>(_config["AnimalServiceUrl"]
                + "/api/animals");
        }
    }
}