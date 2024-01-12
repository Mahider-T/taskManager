
using TaskManager.Models;

namespace TaskManager.DTOs;
public class updateTaskDTO {

    public string? name { get; set; }

    public StatusOfTask? Status { get; set; }

    public DateTime? dueDate { get ;set; } 

    // public Tasks MapToTasks() {

    //     Tasks task = new Tasks();
    //     if (this.name != null ) {task.name = this.name;}
    //     if (this.Status != null) {task.Status = (StatusOfTask)this.Status;}
    //     if (this.dueDate != null) {task.dueDate = (DateTime)this.dueDate;}

    //     return task;
    // }


}