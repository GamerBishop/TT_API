using Domain.Constants;

namespace Application.Users;

public record CurrentUser(string Id, string Email, IEnumerable<string> Roles)
{
    public bool IsEnroledIn(string role) => Roles.Contains(role);
    public bool IsAdministrator => IsEnroledIn(UserRoles.Admin);

    public override string ToString()
    {
        return $"User : [ Id: {Id}, Email: {Email}, Roles: {string.Join(", ", Roles)}]";
    }
}