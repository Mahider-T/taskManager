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

    [HttpGet("getUsers")]
    public async Task<ActionResult<List<User>>> GetUsers() {

        return await _userService.GetUsers();
    }
    [HttpGet("getUsers/{id}")]
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

    [HttpPost("login")]
    public async Task<ActionResult<string>> LoginUser(string email, string password) {

        var loginResult = await _userService.LoginUser(email, password);
        // Console.WriteLine(password);
        // Console.WriteLine(loginResult);
        if(loginResult == null) {return BadRequest("Wrong credentials.");}

        var expirationDate = DateTime.UtcNow.AddHours(1); 

         Response.Cookies.Append("AuthToken", loginResult, new Microsoft.AspNetCore.Http.CookieOptions
        {
            HttpOnly = true,
            Expires = expirationDate,
        });
        return Ok(loginResult);
    }
}

