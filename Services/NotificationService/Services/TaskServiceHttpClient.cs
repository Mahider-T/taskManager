using MongoDB.Entities;
// using TaskService;

namespace SearchService.Services
{
    public class TaskServiceHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public TaskServiceHttpClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<List<Task>> GetTasksForNotificationDB()
        {
            // _config["TaskServiceUrl"] = 
            return await _httpClient.GetFromJsonAsync<List<Task>>("http://localhost:5154"
                + "/api/Tasks");
        }
    }
}