using Application.Projects.DTOs;
using AutoMapper;
using Domain.Model;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
namespace Application.Services.AppServices;

internal class ProjectService(IProjectsRepository projectRepository, ILogger<ProjectService> logger, IMapper mapper) : IProjectService
{
    public async Task<int> CreateProject(CreateProjectDto projectDto)
    {
        logger.LogInformation("Creating project with name {ProjectName}", projectDto.Name);
        var project = mapper.Map<Project>(projectDto);
        var projectId = await projectRepository.CreateAsync(project);
        logger.LogInformation("Created project with id {ProjectId}", projectId);
        return projectId;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllProjects()
    {
        logger.LogInformation("Retrieving all projects");
        var projects = await projectRepository.GetAllAsync();
        logger.LogInformation("Retrieved {ProjectCount} projects", projects.Count());

        var projectsDtos = mapper.Map<IEnumerable<ProjectDto>>(projects);

        return projectsDtos;
    }

    public async Task<ProjectDto?> GetProjectById(int id)
    {
        logger.LogInformation("Retrieving project with id {ProjectId}", id);
        var project = await projectRepository.GetByIdAsync(id);
        if (project == null)
        {
            logger.LogWarning("Project with id {ProjectId} was not found", id);
            return null;
        }
        else
        {
            logger.LogInformation("Retrieved project with id {ProjectId}", id);
        }

        var projectDto = mapper.Map<ProjectDto>(project);

        return projectDto;
    }

    public async Task<bool> UpdateProject(UpdateProjectDto projectDto)
    {
        logger.LogInformation("Updating project with id {ProjectId}", projectDto.Id);
        var project = mapper.Map<Project>(projectDto);
        var result = await projectRepository.UpdateAsync(project);
        if (result)
        {
            logger.LogInformation("Updated project with id {ProjectId}", projectDto.Id);
        }
        else
        {
            logger.LogWarning("Failed to update project with id {ProjectId}", projectDto.Id);
        }
        return result;
    }

    public async Task<bool> DeleteProject(int projectId)
    {
        logger.LogInformation("Deleting project with id {ProjectId}", projectId);
        var project = await projectRepository.GetByIdAsync(projectId);
        if (project == null)
        {
            logger.LogWarning("Project with id {ProjectId} was not found", projectId);
            return false;
        }
        var result = await projectRepository.DeleteAsync(project);
        if (result)
        {
            logger.LogInformation("Deleted project with id {ProjectId}", projectId);
        }
        else
        {
            logger.LogWarning("Failed to delete project with id {ProjectId}", projectId);
        }
        return result;
    }

    public async Task<int> GetProjectsCountCreatedByUser(string userId)
    {
        logger.LogInformation("Retrieving project count created by user with id {UserId}", userId);
        var count = await projectRepository.GetProjectsCountCreatedByUser(userId);
        logger.LogInformation("User with id {UserId} has created {ProjectCount} projects", userId, count);
        return count;
    }
}
