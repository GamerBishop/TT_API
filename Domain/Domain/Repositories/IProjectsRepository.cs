using Domain.Model;

namespace Domain.Repositories;

public interface IProjectsRepository
{
    /// <summary>
    /// Retrieves all projects asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of projects.</returns>
    Task<IEnumerable<Project>> GetAllAsync();

    /// <summary>
    /// Retrieves a project by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the project to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the project, or null if not found.</returns>
    Task<Project?> GetByIdAsync(int id);

    /// <summary>
    /// Creates a new project asynchronously.
    /// </summary>
    /// <param name="project">The project to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the created project.</returns>
    Task<int> CreateAsync(Project project);

    /// <summary>
    /// Updates an existing project asynchronously.
    /// </summary>
    /// <param name="project">The project to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the update was successful or not.</returns>
    Task<bool> UpdateAsync(Project project);

    /// <summary>
    /// Deletes a project asynchronously.
    /// </summary>
    /// <param name="project">The project to delete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the deletion was successful or not.</returns>
    Task<bool> DeleteAsync(Project project);

    /// <summary>
    /// Retrieves the number of projects created by the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of projects created by the user.</returns>
    Task<int> GetProjectsCountCreatedByUser(string userId);
}
