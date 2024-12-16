using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Model;

public class Role : IdentityRole<Guid>
{

    [Key]
    public override Guid Id { get; set; }
    [Required]
    [MaxLength(50)]
    public override required string? Name { get; set; } = string.Empty;
    [NotMapped]
    public virtual ICollection<TeamMember> TeamMembers { get; set; } = [];
    [NotMapped] 
    public virtual ICollection<User> Users { get; set; } = [];
}
