using TaskManager.Models;
using TaskManager.DTOs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using AutoMapper;

namespace TaskManager.Services;

public class TasksService {

    private readonly IMongoCollection<Tasks> _tasksCollection;
    private readonly IMapper _mapper;

    public TasksService(
        IMapper mapper,
        IOptions<TaskManagerDatabaseSettings> taskManagerDatabaseSettings) {
            _mapper = mapper;
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

    //Make sure the below method works properly give the use of an automapper.
    public async Task UpdateAsync(string id, updateTaskDTO updatedTask) {
        Tasks task = _mapper.Map<Tasks>(updatedTask);

        await _tasksCollection.ReplaceOneAsync(x => x.Id == id, task);
    }
        

    public async Task RemoveAsync(string id) =>
        await _tasksCollection.DeleteOneAsync(x => x.Id == id);

}