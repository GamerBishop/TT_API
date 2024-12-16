using Application.Services.AppServices;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TT_API.Controllers
{
    /// <summary>
    /// Contrôleur pour gérer les membres de l'équipe.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize()]
    public class TeamMemberController(IUserService userService) : ControllerBase
    {
        /// <summary>
        /// Affecte un utilisateur à un projet avec un rôle spécifique.
        /// </summary>
        /// <param name="userId">L'identifiant de l'utilisateur.</param>
        /// <param name="projectId">L'identifiant du projet.</param>
        /// <param name="Role">Le rôle à assigner à l'utilisateur.</param>
        /// <returns>Retourne un statut HTTP 200 si l'affectation est réussie.</returns>
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager)]
        [HttpPost("affect")]
        public async Task<IActionResult> AffectUserToProject([FromRoute] Guid userId, [FromRoute] int projectId, [FromRoute] string Role)
        {
            await userService.AssignProject(userId, projectId, Role);
            return Ok();
        }
    }
}
