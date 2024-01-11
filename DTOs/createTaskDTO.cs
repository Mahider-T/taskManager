using TaskManager.Models;

namespace TaskManager.DTOs;

public class CreateTaskDTO {

    public string name { get; set; } = null!;

    public StatusOfTask? Status { get; set; }

    public DateTime? dueDate { get ;set; } 

    public string? userId { get; set; } = null!;

    public Tasks MapToTasks() {
        var tasks = new Tasks
        {
            name = this.name,
            Status = this.Status ?? StatusOfTask.Todo, // Use default if Status is null
            dueDate = this.dueDate ?? DateTime.UtcNow.AddDays(1), // Use default if dueDate is null
            createdAt = DateTime.UtcNow
        };

        return tasks; 
    }
}