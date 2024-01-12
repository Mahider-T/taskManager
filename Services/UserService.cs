using MongoDB.Driver;
using TaskManager.Models;
using TaskManager.DTOs;
using Microsoft.Extensions.Options;
using TaskManager.Helpers;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp.Unsafe;
using Amazon.Runtime;

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


    public async Task<List<User>> GetUsers() {
        return await _usersCollection.Find(_ => true).ToListAsync(); 

    }
    public async Task<User?> CreateAsync(CreateUserDTO newUser){

        // Tasks task = _mapper.Map<Tasks>(newTask);

        PasswordHasher passwordHasher = new PasswordHasher();
        User user = newUser.MapToUser();

        if( user.password is null ) {return null;}
        user.password = passwordHasher.HashPassword(user.password);
        Console.WriteLine(user);
        await _usersCollection.InsertOneAsync(user);
        return user;
    }

    public async Task<User> GetUserAsync(string id) {

        return await _usersCollection.Find(user => user.Id == id).FirstOrDefaultAsync();
    }

    public async Task<UpdateResult> UpdateNameAsync(string id, string newName) {
        var filter = Builders<User>.Filter.Eq(user =>user.Id, id );
        var update = Builders<User>.Update.Set(user => user.name,  newName);

        return await _usersCollection.UpdateOneAsync(filter, update);

        
    }

}