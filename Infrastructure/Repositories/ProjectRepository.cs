using Domain.Model;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class ProjectRepository : IProjectsRepository
{
    private readonly ProjectsDbContext _projectsDbContext;

    public ProjectRepository(ProjectsDbContext projectsDbContext)
    {
        _projectsDbContext = projectsDbContext;
    }

    /// <summary>
    /// Creates a new project asynchronously.
    /// </summary>
    /// <param name="project">The project to create.</param>
    /// <returns>The ID of the created project.</returns>
    public async Task<int> CreateAsync(Project project)
    {
        _projectsDbContext.Projects.Add(project);
        await _projectsDbContext.SaveChangesAsync();
        return project.ProjectId;
    }

    /// <summary>
    /// Deletes a project asynchronously.
    /// </summary>
    /// <param name="project">The project to delete.</param>
    /// <returns>True if the project was deleted, otherwise false.</returns>
    public async Task<bool> DeleteAsync(Project project)
    {
        _projectsDbContext.Projects.Remove(project);
        var result = await _projectsDbContext.SaveChangesAsync();
        return result > 0;
    }

    /// <summary>
    /// Gets all projects asynchronously.
    /// </summary>
    /// <returns>A collection of all projects.</returns>
    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _projectsDbContext.Projects.Include(p => p.TeamMembers).ToListAsync();
    }

    /// <summary>
    /// Gets a project by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the project.</param>
    /// <returns>The project with the specified ID, or null if not found.</returns>
    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _projectsDbContext.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
    }

    /// <summary>
    /// Gets the count of projects created by a specific user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The count of projects created by the user.</returns>
    public async Task<int> GetProjectsCountCreatedByUser(string userId)
    {
        return await _projectsDbContext.Projects.CountAsync(p => p.CreatedBy.ToString() == userId);
    }

    /// <summary>
    /// Updates a project asynchronously.
    /// </summary>
    /// <param name="project">The project to update.</param>
    /// <returns>True if the project was updated, otherwise false.</returns>
    public async Task<bool> UpdateAsync(Project project)
    {
        var existingProject = await _projectsDbContext.Projects.FirstOrDefaultAsync(p => p.ProjectId == project.ProjectId);
        if (existingProject == null)
        {
            return false;
        }

        existingProject.Name = project.Name;
        existingProject.Description = project.Description;
        existingProject.StartDate = project.StartDate;
        existingProject.EndDate = project.EndDate;
        existingProject.CreatedBy = project.CreatedBy;
        existingProject.TeamMembers = project.TeamMembers;

        await _projectsDbContext.SaveChangesAsync();
        return true;
    }
}
