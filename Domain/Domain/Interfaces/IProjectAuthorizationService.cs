using Domain.Constants;
using Domain.Model;

namespace Domain.Interfaces;

public interface IProjectAuthorizationService
{
    bool Authorize(Project project, ResourceOperation operation);
}
