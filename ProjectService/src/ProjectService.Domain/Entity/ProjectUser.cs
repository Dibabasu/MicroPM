using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entity
{
    public class ProjectUser: AuditableEntity
    {
        public string UserName{get;set;}=string.Empty;
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }
        public UserRole UserRole { get; set; }
        
    }
}