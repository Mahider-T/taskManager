namespace Events;

public class TaskUpdated {

    //  public enum StatusOfTask{
    //     Todo,
    //     Underway,
    //     Completed
    // }
    public string oldName { get; set; } 
    public string userId { get; set;}

    public string nameOfUser {get; set; }
    public StatusOfTask oldStatus { get; set; } 

    public DateTime createdAt { get; set; } 
    public DateTime oldDueDate { get ;set; } 
    public DateTime? UpdatedAt { get; set; }

    public string newName { get; set; }
    public StatusOfTask newStatus { get; set; }
    public DateTime newDueDate { get; set; }

    public TaskUpdated () {
        this.UpdatedAt = DateTime.Now;   
    }
}