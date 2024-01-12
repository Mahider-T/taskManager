using Microsoft.AspNetCore.Mvc;
using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase {
    
    private readonly UserService _userService;

    public UserController(UserService userService) {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers() {

        return await _userService.GetUsers();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(string id) {

        return await _userService.GetUserAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUserAsync(CreateUserDTO newUser) {

        
        var user = await _userService.CreateAsync(newUser);
        if (user is not null) {return CreatedAtAction(nameof(GetUserById), new {user.Id}, user);}

        return BadRequest();
        
    }
}

