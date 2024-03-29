using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskManager.Models;
using TaskManager.Helpers;

namespace TaskManager.Services;

public class TokenService {
    private readonly IMongoCollection<Tokens> _tokenCollection;

    public TokenService(IOptions<TaskManagerDatabaseSettings> taskManagerDatabaseSettings) {
        var mongoClient = new MongoClient(
            taskManagerDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            taskManagerDatabaseSettings.Value.DatabaseName);

        _tokenCollection = mongoDatabase.GetCollection<Tokens>(
            taskManagerDatabaseSettings.Value.TokensCollectionName);
    }


    public Tokens GetToken(string email, string name) {

        Tokens theNewToken = new Tokens();
        theNewToken.email = email;
        theNewToken.Token = Tokenize.GenerateToken(email, name);

        return theNewToken;
    }

    public async Task<Tokens> SaveTokenAsync(Tokens token) {
        await _tokenCollection.InsertOneAsync(token);
        return token;
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