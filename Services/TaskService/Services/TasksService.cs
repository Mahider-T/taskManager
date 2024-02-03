using TaskManager.Models;
using TaskManager.DTOs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using AutoMapper;
using TaskManager.Helpers;

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
        // Console.WriteLine(newTask.Status);

        var tasks = new Tasks
        {
            // ?? StatusOfTask.Underway
            name = newTask.name,
            // Status = (TaskManager.Models.StatusOfTask)newTask.Status , // Use default if Status is null
            Status = newTask.Status != null ? EnumHelper.EnumParse(newTask.Status, StatusOfTask.Todo) : StatusOfTask.Todo,
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

        var theTask = await GetAsync(id);

        if (updatedTask.name != null)
        {
            updateDefinitionList.Add(updateDefinitionBuilder.Set(x => x.name, updatedTask.name));
        }

        if (updatedTask.Status != null)
        {
            StatusOfTask theStatus = EnumHelper.EnumParse(updatedTask.Status,theTask!.Status)!;
            // updateDefinitionList.Add(updateDefinitionBuilder.Set(x => x.Status, updatedTask.Status));
            updateDefinitionList.Add(updateDefinitionBuilder.Set(x => x.Status, theStatus));
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