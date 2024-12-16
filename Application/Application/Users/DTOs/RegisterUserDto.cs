using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Application.Users.DTOs;

public class RegisterUserDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}
