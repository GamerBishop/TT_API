using Domain.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Application.Projects.DTOs;

public class ProjectDto
{
    public required string Name { get; set; }

    public required string Description { get; set; }

    public required DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public User CreatedBy { get; set; } = default!;

    public List<TeamMember> TeamMembers { get; set; } = [];
}

