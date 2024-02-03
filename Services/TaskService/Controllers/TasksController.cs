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
using Events;

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
        // var claims = principal.Claims;
        var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")!;
        var nameClaim = principal.Claims.FirstOrDefault(c => c.Type == "name")!;
        // Console.WriteLine(typeof(principal.Claims));
        // foreach (Claim claim in principal.Claims)
        // {
        //     Console.WriteLine(claim.Type);
        //     Console.WriteLine(claim.Value);
        
        //     // Process claim based on type and value
        // }
        string email = emailClaim.Value;
        string name = nameClaim.Value;
        Console.WriteLine(name);

        var task = await _tasksService.CreateAsync(newTask, email);
        TaskCreated taskCreated = new TaskCreated{
            Id = task.Id,
            name = task.name,
            nameOfUser = name,
            Status = (Events.TaskCreated.StatusOfTask)task.Status!,
            createdAt = task.createdAt,
            dueDate = task.dueDate,
            userId  = task.userId
        };

        await _publishEndpoint.Publish(taskCreated);
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

        var name = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "name")!.Value;

        await _tasksService.UpdateAsync(id, updatedTask);

        var updatedTaskData = await _tasksService.GetAsync(id);

        if (updatedTaskData is null ) {
            return NotFound("Could not find the updated version.");
        }

        TaskUpdated taskUpdated = new TaskUpdated {
            oldName = theTask.name,
            userId = theTask.userId,
            nameOfUser = name,
            oldStatus = (Events.StatusOfTask)theTask.Status,
            createdAt = theTask.createdAt,
            oldDueDate = theTask.dueDate,

            newName = updatedTaskData.name,
            newStatus = (Events.StatusOfTask)updatedTask.Status!,
            newDueDate = updatedTaskData.dueDate

        };
        

        await _publishEndpoint.Publish(taskUpdated);

        // TaskUpdated updatedTaskEvent = new TaskUpdated{

        // }

        return Ok(updatedTaskData);

    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<Tasks>> DeleteTask(String id) {

        var theTask = await _tasksService.GetAsync(id);

        if(theTask is null) {
            return NotFound("Could not find a task with the given Id");
        }
        await _publishEndpoint.Publish(id);
        return Ok("Task deleted successfully.");

    }


}