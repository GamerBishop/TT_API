
using Application.Users.DTOs;

namespace Application.Services.AppServices
{
    public interface IUserService
    {
        Task<bool> AddRole(Guid userId, string role);
        Task<Guid> AddUser(string userName, string email, string password, string role);
        Task<bool> AssignProject(Guid userId, string projectId, string role);
        Task<bool> UnassignRole(string userId, string role);
        Task<UserDto> GetUserByName(string userName);
        Task<UserDto> GetUserByEmail(string userMail);
    }
}