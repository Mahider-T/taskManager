using MongoDB.Driver;
using TaskManager.Models;
using TaskManager.DTOs;
using Microsoft.Extensions.Options;

namespace TaskManager.Services;

public class UserService {
    private readonly IMongoCollection<User> _usersCollection;

    public UserService(IOptions<TaskManagerDatabaseSettings> taskManagerDatabaseSettings) {
        var mongoClient = new MongoClient(
            taskManagerDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            taskManagerDatabaseSettings.Value.DatabaseName);

        _usersCollection = mongoDatabase.GetCollection<User>(
            taskManagerDatabaseSettings.Value.UsersCollectionName);
    }

    public async Task<User> CreateAsync(CreateUserDTO newUser){

        // Tasks task = _mapper.Map<Tasks>(newTask);
        User user = newUser.MapToUser();
        await _usersCollection.InsertOneAsync(user);
        return user;
    }

    public async Task<User> UpdateAsync(UpdateUserDTO updatedUser) {
        
    }

}