using TaskManager.Models;

namespace TaskManager.DTOs;

public class CreateTaskDTO {

    public string name { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? dueDate { get ;set; } 
}