namespace Application.Users.DTOs;

public class UserDto
{
    public required string UserName { get; set; }
    public required string Email { get; set; } 
    public required List<string> Roles { get; set; }
}
