using Domain.Model;

namespace Application.Projects.DTOs;

public class CreateProjectDto
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public required string Description { get; set; }

    public required DateTime StartDate { get; set; }

    public User CreatedBy { get; set; } = default!;
}
