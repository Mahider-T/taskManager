using TaskManager.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace TaskManager.Services;

public class TasksService {

    private readonly IMongoCollection<Tasks> _tasksCollection;

    public TasksService(
        IOptions<TaskManagerDatabaseSettings> taskManagerDatabaseSettings) {
         var mongoClient = new MongoClient(
            taskManagerDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            taskManagerDatabaseSettings.Value.DatabaseName);

        _tasksCollection = mongoDatabase.GetCollection<Tasks>(
            taskManagerDatabaseSettings.Value.TasksCollectionName);
    }
    public async Task<List<Tasks>> GetAsync() =>
        await _tasksCollection.Find(_ => true).ToListAsync();

    public async Task<Tasks?> GetAsync(string id) =>
        await _tasksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Tasks newTasks) =>
        await _tasksCollection.InsertOneAsync(newTasks);

    public async Task UpdateAsync(string id, Tasks updatedTasks) =>
        await _tasksCollection.ReplaceOneAsync(x => x.Id == id, updatedTasks);

    public async Task RemoveAsync(string id) =>
        await _tasksCollection.DeleteOneAsync(x => x.Id == id);

}