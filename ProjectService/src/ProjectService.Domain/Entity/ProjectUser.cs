using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entity
{
    public class ProjectUser: AuditableEntity
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }
        public UserRole UserRole { get; set; }
        
    }
}