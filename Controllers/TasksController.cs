using TaskManager.Models;
using TaskManager.Services;
using TaskManager.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using DnsClient.Protocol;
using Microsoft.AspNetCore.Authorization;

namespace TaskManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase {

    private readonly TasksService _tasksService;

    public TasksController(TasksService tasksService) {
        _tasksService = tasksService;
    }

    //Interesting fact : [Authorize] does not return 401(UnAuthorized) or 403(Forbidden)
    //when an UnAuthenticatd user tries to access a resource but rather a 404(NotFound)
    //for security measures.
    [HttpGet]
    [Authorize]
    public async Task<List<Tasks>> GetTask() {

        return await _tasksService.GetAsync();

    } 

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Tasks>> GetTaskById(string id) {
        var theTask = await _tasksService.GetAsync(id);

        if(theTask is null) return NotFound();

        return theTask;

    }

    [HttpPost]
    public async Task<ActionResult<Task>> PostTask(CreateTaskDTO newTask) {

        var task = await _tasksService.CreateAsync(newTask); 

        return CreatedAtAction(nameof(GetTaskById), new {task.Id}, task);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Task>> UpdateTask(string id, updateTaskDTO updatedTask) {

        var theTask = await _tasksService.GetAsync(id);

        if(theTask is null) {
            return NotFound("Could not find a task with the given Id");
        }

        await _tasksService.UpdateAsync(id, updatedTask);
        var updatedTaskData = await _tasksService.GetAsync(id);

        return Ok(updatedTaskData);

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