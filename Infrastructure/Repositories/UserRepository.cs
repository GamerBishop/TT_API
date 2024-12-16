using Domain.Repositories;
using Domain.Exceptions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

internal class UserRepository(ProjectsDbContext _projectsDbContext, UserManager<User> _userManager, RoleManager<Role> _roleManager) : IUserRepository
{ 

    /// <summary>
    /// Adds a new role to the user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="role">The role to add.</param>
    /// <returns>True if the role was added successfully, otherwise false.</returns>
    public async Task<bool> AddRole(Guid userId, string role)
    {
        var user = await _projectsDbContext.Users.FindAsync(userId) 
            ?? throw new NotFoundException("user", userId.ToString());

        var roleEntity = await _projectsDbContext.Roles.FirstOrDefaultAsync(r => r.Name == role) 
            ?? throw new NotFoundException("role", role);

        var result = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded)
        {
            throw new Exception("Failed to add role: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return true;
    }
    

    /// <summary>
    /// Adds a new user to the database.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="email">The email of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <param name="role">The role of the user.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public async Task<Guid> AddUser(string userName, string email, string password, string role)
    {
        var user = new User
        { 
            Email = email,
            UserName = userName
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new IdentityException("Failed to create user: " , result.Errors);
        }

        var roleEntity = await _projectsDbContext.Roles.FirstOrDefaultAsync(r => r.Name == role) ?? throw new NotFoundException("role", role);
        user.Roles.Add(roleEntity);
        await _projectsDbContext.SaveChangesAsync();
        return user.Id;
    }

    /// <summary>
    /// Assigns a project to a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="projectId">The ID of the project.</param>
    /// <returns>True if the project was assigned successfully, otherwise false.</returns>
    public async Task<bool> AssignProject(Guid userId, string projectId, string role)
    {
        var user = await _projectsDbContext.Users.FindAsync(userId) ?? throw new NotFoundException("user", userId.ToString());

        var project = await _projectsDbContext.Projects.FindAsync(int.Parse(projectId)) ?? throw new NotFoundException("project", projectId);

        var roleEntity = await _projectsDbContext.Roles.FirstOrDefaultAsync(r => r.Name == role);
        if (roleEntity == null) {
            roleEntity = new Role { Name = role };
            _projectsDbContext.Roles.Add(roleEntity);
        }

        //Create related TeamMemberEntity
        var teamMember = new TeamMember
        {
            Project = project,
            User = user,
            Role = roleEntity
        };

        project.TeamMembers.Add(teamMember);

        user.Projects.Add(project);
        await _projectsDbContext.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Unassigns a role from a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="role">The role to unassign.</param>
    /// <returns>True if the role was unassigned successfully, otherwise false.</returns>
    public async Task<bool> UnassignRole(string userId, string role)
    {
        var user = await _projectsDbContext.Users.FindAsync(Guid.Parse(userId)) ?? throw new NotFoundException("user", userId);

        var roleEntity = user.Roles.FirstOrDefault(r => r.Name == role) ?? throw new NotFoundException("role", role);

        user.Roles.Remove(roleEntity);
        await _projectsDbContext.SaveChangesAsync();
        return true;
    }
    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The user with the specified username.</returns>
    public async Task<User> GetUserByName(string userName)
    {
        var user = await _projectsDbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        if (user == null)
        {
            throw new NotFoundException("user", userName);
        }
        return user;
    }

    /// <summary>
    /// Retrieves a user by their email.
    /// </summary>
    /// <param name="userMail">The email of the user.</param>
    /// <returns>The user with the specified email.</returns>
    public async Task<User> GetUserByEmail(string userMail)
    {
        var user = await _projectsDbContext.Users.FirstOrDefaultAsync(u => u.Email == userMail);
        if (user == null)
        {
            throw new NotFoundException("user", userMail);
        }
        return user;
    } 
}
