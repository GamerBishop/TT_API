using Application.Users;
using Domain.Constants;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Authorization.Services
{
    public class ProjectsAuthorizationService(ILogger<ProjectsAuthorizationService> logger, IUserContext userContext) : IProjectAuthorizationService
    {
        public bool Authorize(Project project, ResourceOperation operation)
        {
            CurrentUser currentUser = userContext.GetCurrentUser();
            logger.LogInformation($"Authorizing currentUser {currentUser} to {operation} project {project.ProjectId}");

            switch (operation)
            {
                case ResourceOperation.Create:
                case ResourceOperation.Delete:
                    return currentUser.IsAdministrator;

                case ResourceOperation.Update:
                    return currentUser.IsAdministrator || currentUser.IsEnroledIn(UserRoles.Manager);

                case ResourceOperation.Read:
                    return currentUser.IsAdministrator || project.TeamMembers.Any(tm => tm.UserId == Guid.Parse(currentUser.Id));

                default:
                    return false;
            }
        }
    }
}
