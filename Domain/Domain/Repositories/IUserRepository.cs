using Domain.Model;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<Guid> AddUser(string userName,string email, string password, string role);
    Task<bool> AddRole(Guid userId, string role);
    Task<bool> UnassignRole(string userId, string role);
    Task<bool> AssignProject(Guid userId, string projectId, string role);
    Task<User> GetUserByName(string userName);
    Task<User> GetUserByEmail(string userMail);
}
