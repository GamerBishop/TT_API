using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Services.AppServices;
using Application.Projects.DTOs;

namespace TT_API.Controllers
{
    /// <summary>
    /// Contrôleur pour gérer les opérations liées aux projets.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController(IProjectService projectService, IUserService userService) : ControllerBase
    {
        /// <summary>
        /// Récupère tous les projets.
        /// </summary>
        /// <returns>Une liste de projets.</returns>
        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await projectService.GetAllProjects();
            return Ok(projects);
        }

        /// <summary>
        /// Crée un nouveau projet.
        /// </summary>
        /// <param name="createProjectDto">Les informations du projet à créer.</param>
        /// <returns>Le résultat de la création du projet.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProject(CreateProjectDto createProjectDto)
        {
            int resultId = await projectService.CreateProject(createProjectDto);
            return CreatedAtAction(nameof(GetById), new { id = resultId }, null);
        }

        /// <summary>
        /// Met à jour un projet existant.
        /// </summary>
        /// <param name="id">L'identifiant du projet à mettre à jour.</param>
        /// <param name="updateProjectDto">Les nouvelles informations du projet.</param>
        /// <returns>Un statut NoContent si la mise à jour est réussie.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdateProject([FromRoute] int id, UpdateProjectDto updateProjectDto)
        {
            updateProjectDto.Id = id;
            await projectService.UpdateProject(updateProjectDto);
            return NoContent();
        }

        /// <summary>
        /// Récupère un projet par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du projet à récupérer.</param>
        /// <returns>Les informations du projet.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var project = await projectService.GetProjectById(id);
            return Ok(project);
        }
    }
}
