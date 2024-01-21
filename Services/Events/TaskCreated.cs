namespace Events;

public class TaskCreated {
    public string Id { get; set; } 

    public string name { get; set; } 

    public StatusOfTask Status { get; set; }

    public DateTime createdAt { get; set; }
    public DateTime dueDate { get ;set; } 

    public string userId { get; set; } 

}
