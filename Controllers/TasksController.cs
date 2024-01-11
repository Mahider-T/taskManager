using TaskManager.Models;
using TaskManager.Services;
using TaskManager.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TaskManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase {

    private readonly TasksService _tasksService;

    public TasksController(TasksService tasksService) {
        _tasksService = tasksService;
    }

    
    [HttpGet]
    public async Task<List<Tasks>> GetTask() {

        return await _tasksService.GetAsync();

    } 

    [HttpGet("{id}")]
    public async Task<ActionResult<Tasks>> GetTaskById(string id) {
        var theTask = await _tasksService.GetAsync(id);

        if(theTask is null) return NotFound();

        return theTask;

    }

    [HttpPost]
    public async Task<ActionResult<Task>> PostTask(Tasks newTask) {

        await _tasksService.CreateAsync(newTask); 

        return CreatedAtAction(nameof(GetTaskById), new {newTask.Id}, newTask);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Task>> UpdateTask(string id, updateTaskDTO updatedTask) {

        var theTask = await _tasksService.GetAsync(id);

        if(theTask is null) {
            return NotFound("Could not find a task with the given Id");
        }

        await _tasksService.UpdateAsync(id, updatedTask);

        return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Tasks>> DeleteTask(String id) {

        var theTask = await _tasksService.GetAsync(id);

        if(theTask is null) {
            return NotFound("Could not find a task with the given Id");
        }

        return Ok("Task deleted successfully.");

    }


}