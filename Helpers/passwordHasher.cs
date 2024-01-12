
using BCrypt.Net;
namespace TaskManager.Helpers;

public class PasswordHasher {

    public static string HashPassword(string password) {

        string salt = BCrypt.Net.BCrypt.GenerateSalt();

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

        return hashedPassword;
    }

    public static bool verifyPassword(string password, string hashedPassword) {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

}