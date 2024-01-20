using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
public class Tokens
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = null!;

    public string email { get; set; } = null!;
    public string Token { get; set; } = null!;
}