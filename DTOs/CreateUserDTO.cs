
using TaskManager.Models;
namespace TaskManager.DTOs;

public class CreateUserDTO {

    public string name = null!;
    public string email = null!;
    public string password = null!;

    public User MapToUser() {

        var user = new User
        {
            name = this.name,
            email = this.email,
            password = this.password
            
        };

        Console.Write(user.email);

        return user; 
    }
}