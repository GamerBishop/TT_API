using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model;

public class TeamMember
{
    [Key, Column(Order = 0)]
    [ForeignKey("Project")]
    public int ProjectId { get; set; }
    public virtual Project Project { get; set; } = default!;

    [Key, Column(Order = 1)]
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;

    [Key, Column(Order = 2)]
    [ForeignKey("Role")]
    public Guid RoleId { get; set; }
    public virtual Role Role { get; set; } = default!;
}
