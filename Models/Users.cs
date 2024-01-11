using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TaskManager.Models;

public class User {

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public string? name { get; set; }
    public string email { get; set; } = null!;
    public string password { get; set;} = null!;
}