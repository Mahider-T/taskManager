using MongoDB.Driver;
using TaskManager.Models;
using TaskManager.Helpers;
using TaskManager.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp.Unsafe;
using Amazon.Runtime;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TaskManager.Services;

public class UserService {
    private readonly IMongoCollection<User> _usersCollection;
    private readonly TokenService _tokenService;

    public UserService(
        IOptions<TaskManagerDatabaseSettings> taskManagerDatabaseSettings,
        TokenService tokenService) {
        _tokenService = tokenService;
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
    public async Task<User?> CreateAsync(User newUser){

        // Tasks task = _mapper.Map<Tasks>(newTask);
        // User user  = newUser.MapToUser();
        var userExists = await _usersCollection.Find(user => user.email == newUser.email).FirstOrDefaultAsync();

        if (userExists != null) {
            return null;
        }
        else{
            newUser.password = PasswordHasher.HashPassword(newUser.password); 
            await _usersCollection.InsertOneAsync(newUser);

            return newUser;
        }
        
    }

    public async Task<User> GetUserAsync(string id) {

        return await _usersCollection.Find(user => user.Id == id).FirstOrDefaultAsync();
    }

    public async Task<UpdateResult> UpdateNameAsync(string id, string newName) {
        var filter = Builders<User>.Filter.Eq(user =>user.Id, id );
        var update = Builders<User>.Update.Set(user => user.name,  newName);

        return await _usersCollection.UpdateOneAsync(filter, update);

    }

    public async Task<string?> LoginUser(string email, string password){ 

        User userExists = await _usersCollection.Find(user => user.email == email).FirstOrDefaultAsync();

        // Console.WriteLine("Email is : " + email);
        // Console.WriteLine("Password is : " + password);
        
        Console.WriteLine("Must be false => " + userExists == null);

        if(userExists == null) {return null;}

        Console.WriteLine("Must be false => " + userExists.password != password);

        Boolean passwordMatches = PasswordHasher.verifyPassword(password, userExists.password);

        if (!passwordMatches) {return null;}

        Tokens theToken = _tokenService.GetToken(email);

        Console.WriteLine("The token is => " + theToken);

        return theToken.Token;

    }

}