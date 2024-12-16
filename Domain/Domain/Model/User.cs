using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Domain.Model;

public class User : IdentityUser<Guid>
{
    [Key()]
    public override Guid Id { get; set; }

    [MaxLength(100)]
    public override string? UserName { get; set; } = String.Empty;

    [EmailAddress]
    [MaxLength(100)]
    public override string? Email { get; set; } = String.Empty;

    public override string? PasswordHash { get; set; } = String.Empty;

    public List<Role> Roles { get; set; } = [];

    [NotMapped]
    [JsonIgnore]
    public virtual ICollection<TeamMember> TeamMembers { get; set; } = [];
    [NotMapped]
    [JsonIgnore]
    public virtual ICollection<Project> Projects { get; set; } = [];
}
