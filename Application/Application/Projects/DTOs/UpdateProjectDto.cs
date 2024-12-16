using Domain.Model;

namespace Application.Projects.DTOs;

public class UpdateProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public User CreatedBy { get; set; } = default!;
    public List<TeamMember> TeamMembers { get; set; } = [];
}
