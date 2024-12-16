using Application.Projects.DTOs;

namespace Application.Services.AppServices
{
    internal interface IProjectService
    {
        Task<int> CreateProject(CreateProjectDto projectDto);
        Task<bool> DeleteProject(int projectId);
        Task<IEnumerable<ProjectDto>> GetAllProjects();
        Task<ProjectDto?> GetProjectById(int id);
        Task<int> GetProjectsCountCreatedByUser(string userId);
        Task<bool> UpdateProject(UpdateProjectDto projectDto);
    }
}