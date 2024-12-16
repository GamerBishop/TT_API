using Domain.Constants;
using Domain.Model;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Seeders;

internal class ProjectSeeder(ProjectsDbContext dbContext, UserManager<User> userManager) : IProjectSeeder
{
    public async Task Seed()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            if(!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }
            if (!dbContext.Users.Any())
            {
                var users = GetUsers();

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, user.Email!); //Faire des emails avec un chiffre et une majuscule
                } 
                await dbContext.SaveChangesAsync();
            }

            //Guid of Admin user 
            Guid adminGuid = dbContext.Users.FirstOrDefault(u => u.Roles.Any(r => r.Name == UserRoles.Admin))!.Id;
            // Guid of Manager user
            Guid managerGuid = dbContext.Users.FirstOrDefault(u => u.Roles.Any(r => r.Name == UserRoles.Manager))!.Id;
            // Guid of Viewer user
            Guid viewerGuid = dbContext.Users.FirstOrDefault(u => u.Roles.Any(r => r.Name == UserRoles.Viewer))!.Id;
            
            if (!dbContext.Projects.Any())
            {
                var projects = GetProjects(adminGuid); 
                dbContext.Projects.AddRange(projects);
                await dbContext.SaveChangesAsync();
            }
            // Id of the projects
            int projectId1 = dbContext.Projects.FirstOrDefault(p => p.Name == "Project 1")!.ProjectId;
            int projectId2 = dbContext.Projects.FirstOrDefault(p => p.Name == "Project 2")!.ProjectId;
            int projectId3 = dbContext.Projects.FirstOrDefault(p => p.Name == "Project 3")!.ProjectId;

            if(!dbContext.TeamMembers.Any())
            {
                var teamMembers = GetTeamMembers(adminGuid, managerGuid, viewerGuid, projectId1, projectId2, projectId3);
                dbContext.TeamMembers.AddRange(teamMembers);
                await dbContext.SaveChangesAsync();
            }
        }
}

    private TeamMember[] GetTeamMembers(Guid adminGuid, Guid managerGuid, Guid viewerGuid, int projectId1, int projectId2, int projectId3)
    {
        //Create teams by affecting users to projects with roles
        return new TeamMember[]
        {
            new TeamMember
            {
                ProjectId = projectId1,
                UserId = adminGuid,
                RoleId = dbContext.Roles.FirstOrDefault(r => r.Name == UserRoles.Admin)!.Id
            },
            new TeamMember
            {
                ProjectId = projectId1,
                UserId = managerGuid,
                RoleId = dbContext.Roles.FirstOrDefault(r => r.Name == UserRoles.Manager)!.Id
            },
            new TeamMember
            {
                ProjectId = projectId1,
                UserId = viewerGuid,
                RoleId = dbContext.Roles.FirstOrDefault(r => r.Name == UserRoles.Viewer)!.Id
            },
            new TeamMember
            {
                ProjectId = projectId2,
                UserId = adminGuid,
                RoleId = dbContext.Roles.FirstOrDefault(r => r.Name == UserRoles.Admin)!.Id
            },
            new TeamMember
            {
                ProjectId = projectId2,
                UserId = managerGuid,
                RoleId = dbContext.Roles.FirstOrDefault(r => r.Name == UserRoles.Manager)!.Id
            },
            new TeamMember
            {
                ProjectId = projectId3,
                UserId = adminGuid,
                RoleId = dbContext.Roles.FirstOrDefault(r => r.Name == UserRoles.Admin)!.Id
            }
        };
    }

    private User[] GetUsers()
    {
        return new User[]
        {
            new User
            {
                UserName = "admin",
                Email = "Admin1@example.com",
                Roles = new List<Role> {
                    new Role { Name = UserRoles.Admin }
                }
            },
            new User
            {
                UserName = "manager",
                Email = "Manager1@example.com",
                Roles = new List<Role> {
                    new Role { Name = UserRoles.Manager }
                }
            },
            new User
            {
                UserName = "viewer",
                Email = "Viewer1@example.com",
                Roles = new List<Role> {
                    new Role { Name = UserRoles.Viewer }
                }
            }
        };
    }

    private Role[] GetRoles()
    {
        return [
            new Role { Name = UserRoles.Admin },
            new Role { Name = UserRoles.Manager },
            new Role { Name = UserRoles.Viewer }
            ];
    }

    private Project[] GetProjects(Guid adminGuid)
    {
        return [
            new Project
            {
                Name = "Project 1",
                Description = "Description of project 1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                CreatedBy = adminGuid
            },
            new Project
            {
                Name = "Project 2",
                Description = "Description of project 2",
                StartDate = DateTime.Now,
                CreatedBy = adminGuid
            },
            new Project
            {
                Name = "Project 3",
                Description = "Description of project 3",
                StartDate = DateTime.Now,
                CreatedBy = adminGuid,
                EndDate = DateTime.Now.AddDays(10)
            }
        ];
    }
}
