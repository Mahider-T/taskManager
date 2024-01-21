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

    public async Task<Tasks> CreateAsync(CreateTaskDTO newTask, string email){

        var tasks = new Tasks
        {
            name = newTask.name,
            Status = newTask.Status ?? StatusOfTask.Todo, // Use default if Status is null
            dueDate = newTask.dueDate ?? DateTime.UtcNow.AddDays(1), // Use default if dueDate is null
            createdAt = DateTime.UtcNow
        };

        // Tasks task = newTask.MapToTasks();
        tasks.userId = email;
        await _tasksCollection.InsertOneAsync(tasks);
        return tasks;
    
    }
        

    //Make sure the below method works properly give the use of an automapper.
    public async Task UpdateAsync(string id, updateTaskDTO updatedTask) {
        // Tasks task = _mapper.Map<Tasks>(updatedTask);
        // Tasks task = updatedTask.MapToTasks();
      var updateDefinitionBuilder = Builders<Tasks>.Update;
        var updateDefinitionList = new List<UpdateDefinition<Tasks>>();

        if (updatedTask.name != null)
        {
            updateDefinitionList.Add(updateDefinitionBuilder.Set(x => x.name, updatedTask.name));
        }

        if (updatedTask.Status.HasValue)
        {
            updateDefinitionList.Add(updateDefinitionBuilder.Set(x => x.Status, updatedTask.Status));
        }

        if (updatedTask.dueDate.HasValue)
        {
            updateDefinitionList.Add(updateDefinitionBuilder.Set(x => x.dueDate, updatedTask.dueDate.Value));
        }

        var updateDefinition = updateDefinitionBuilder.Combine(updateDefinitionList);

        if (updateDefinitionList.Count > 0)
        {
            var filter = Builders<Tasks>.Filter.Eq(x => x.Id, id);
            await _tasksCollection.UpdateOneAsync(filter, updateDefinition);
        }

    }
        

    public async Task RemoveAsync(string id) =>
        await _tasksCollection.DeleteOneAsync(x => x.Id == id);

}