namespace Events;

public class TaskDeleted  {
    public string name { get; set; } 

    public string userId{ get; set; }

    public string nameOfUser {get; set; }
    public StatusOfTask Status { get; set; } 

    public DateTime createdAt { get; set; } 
    public DateTime dueDate { get ;set; } 
    

}