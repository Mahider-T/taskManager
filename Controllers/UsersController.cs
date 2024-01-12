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
    public async Task<ActionResult<User>> CreateUserAsync(User newUser) {

        
       var result =  await _userService.CreateAsync(newUser);
        
        if (result is null) {
            return BadRequest("User with this email already exists!");
        }

        return Ok("User created successfully!");
        
    }

    [HttpPost]
    public async Task<ActionResult<string>> LoginUser(string email, string password) {

        var loginResult = await _userService.LoginUser(email, password);
        if(loginResult == null) {return BadRequest("Terrible request my friend.");}

        return Ok(loginResult);
    }
}

