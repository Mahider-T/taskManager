using TaskManager.Models;
using TaskManager.Services;
using TaskManager.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using DnsClient.Protocol;
using Microsoft.AspNetCore.Authorization;
using Amazon.Auth.AccessControlPolicy;
using System.Security.Claims;
using MassTransit;

namespace TaskManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase {

    private readonly TasksService _tasksService;
    private readonly IPublishEndpoint _publishEndpoint;

    public TasksController(TasksService tasksService, IPublishEndpoint publishEndpoint) {
        _tasksService = tasksService;
        _publishEndpoint = publishEndpoint; 
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
    [Authorize]
    public async Task<ActionResult<Task>> CreateTask(CreateTaskDTO newTask) {

        var principal = HttpContext.User;
        var claims = principal.Claims;
        var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")!;
        string email = emailClaim.Value;
        Console.WriteLine(email);

        var task = await _tasksService.CreateAsync(newTask, email!); 

        await _publishEndpoint.Publish(newTask);
        // return Ok("Task created successfully!");
        return CreatedAtAction(nameof(GetTaskById), new {task.Id}, task);
    }

    [HttpPut("{id}")]
    [Authorize]
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
    [Authorize]
    public async Task<ActionResult<Tasks>> DeleteTask(String id) {

        var theTask = await _tasksService.GetAsync(id);

        if(theTask is null) {
            return NotFound("Could not find a task with the given Id");
        }

        return Ok("Task deleted successfully.");

    }


}