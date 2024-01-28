namespace Events;

public class TaksUpdated {
    public string name { get; set; } 

    public string nameOfUser {get; set; }
    public StatusOfTask Status { get; set; } 

    public DateTime createdAt { get; set; } 
    public DateTime dueDate { get ;set; } 
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}