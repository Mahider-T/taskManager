
namespace TaskManager.DTOs;
public class updateTaskDTO {

    public string? name { get; set; }

    public TaskStatus? Status { get; set; }

    public DateTime? dueDate { get ;set; } 

    public string? userId { get; set; } 
}