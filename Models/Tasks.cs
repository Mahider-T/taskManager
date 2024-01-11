using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TaskManager.Models;

public class Tasks {

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public string name { get; set; } = null!;

    public TaskStatus Status { get; set; }

    public DateTime createdAt { get; set; }
    public DateTime dueDate { get ;set; } 

    public string userId { get; set; } = null!;

    public Tasks() {
        Status = TaskStatus.Todo;
        createdAt = DateTime.UtcNow;
        dueDate = DateTime.UtcNow.AddDays(1);

    }

    

}