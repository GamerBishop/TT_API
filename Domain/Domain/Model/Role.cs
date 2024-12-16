using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model;

public class Role : IdentityRole<Guid>
{

    [Key]
    public override Guid Id { get; set; }
    [Required]
    [MaxLength(50)]
    public override required string? Name { get; set; } = string.Empty;

    public virtual ICollection<TeamMember> TeamMembers { get; set; } = [];
}
