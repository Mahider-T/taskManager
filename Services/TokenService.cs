using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskManager.Models;

namespace TaskManager.Services;

public class TokenService {
    private readonly IMongoCollection<Tokens> _tokenCollection;

    public TokenService(IOptions<TaskManagerDatabaseSettings> taskManagerDatabaseSettings) {
        var mongoClient = new MongoClient(
            taskManagerDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            taskManagerDatabaseSettings.Value.DatabaseName);

        _tokenCollection = mongoDatabase.GetCollection<Tokens>(
            taskManagerDatabaseSettings.Value.UsersCollectionName);
    }


    public async Task<Tokens> SaveTokenAsync(string email, string token) {

        Tokens theNewToken = new Tokens();
        theNewToken.email = email;
        theNewToken.Token = token;

        await _tokenCollection.InsertOneAsync(theNewToken);

        return theNewToken;
    }

    public Tokens FindToken(string email)
    {
        return _tokenCollection.Find(t => t.email == email).FirstOrDefault();
    }

    public void RevokeToken(string email)
    {
        _tokenCollection.DeleteOne(t => t.email == email);
    }
}