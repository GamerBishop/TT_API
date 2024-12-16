using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model;

    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public required string Description { get; set; }

        [Required]
        public required DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [ForeignKey("User")]
        public required Guid CreatedBy { get; set; }
        public virtual User CreatedBy_User { get; set; } = default!;
        
        public virtual ICollection<TeamMember> TeamMembers { get; set; } = [];
}