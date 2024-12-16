using Application.Users.DTOs;
using AutoMapper;
using Domain.Exceptions;
using Domain.Model;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.AppServices;

/// <summary>
/// Service for managing users, roles, and project assignments.
/// </summary>
public class UserService(IUserRepository _userRepository, IEmailSender<User> _emailSender, IConfiguration _configuration, ILogger<UserService> _logger, IMapper mapper) : IUserService
{
    /// <summary>
    /// Adds a new user with the specified email, password, and role.
    /// </summary>
    /// <param name="email">The email of the new user.</param>
    /// <param name="password">The password for the new user.</param>
    /// <param name="role">The role to assign to the new user.</param>
    /// <returns>The ID of the newly created user.</returns>
    /// <exception cref="Exception">Thrown when user creation fails.</exception>
    public async Task<Guid> AddUser(string userName, string email, string password, string role)
    {
        _logger.LogInformation("Adding a new user with email: {Email}", email);

        Guid result = await _userRepository.AddUser(userName, email, password, role);

        _logger.LogInformation("User created successfully with ID: {UserId}", result);

        return result;
    }

    /// <summary>
    /// Adds a role to the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="role">The role to add.</param>
    /// <returns>True if the role was added successfully, otherwise false.</returns>
    public async Task<bool> AddRole(Guid userId, string role)
    {
        _logger.LogInformation("Adding role {Role} to user with ID: {UserId}", role, userId);
        return await _userRepository.AddRole(userId, role);
    }

    /// <summary>
    /// Unassigns a role from the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="role">The role to unassign.</param>
    /// <returns>True if the role was unassigned successfully, otherwise false.</returns>
    public async Task<bool> UnassignRole(string userId, string role)
    {
        _logger.LogInformation("Unassigning role {Role} from user with ID: {UserId}", role, userId);
        return await _userRepository.UnassignRole(userId, role);
    }

    /// <summary>
    /// Assigns a project to the specified user with a given role.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="projectId">The ID of the project.</param>
    /// <param name="role">The role to assign in the project.</param>
    /// <returns>True if the project was assigned successfully, otherwise false.</returns>
    public async Task<bool> AssignProject(Guid userId, string projectId, string role)
    {
        _logger.LogInformation("Assigning project {ProjectId} to user with ID: {UserId} and role: {Role}", projectId, userId, role);
        return await _userRepository.AssignProject(userId, projectId, role);
    }

    public async Task<UserDto> GetUserByName(string userName)
    {
        _logger.LogInformation("Getting user by name: {UserName}", userName);
        User result = await _userRepository.GetUserByName(userName) ?? throw new NotFoundException("user", userName);
        return mapper.Map<UserDto>(result!);
    }

    public async Task<UserDto> GetUserByEmail(string userMail)
    {
        _logger.LogInformation("Getting user by email: {UserMail}", userMail);
        User result = await _userRepository.GetUserByEmail(userMail) ?? throw new NotFoundException("user", userMail);
        UserDto resultMap = mapper.Map<UserDto>(result!);
        return resultMap;
    }
}
