using CSharpFunctionalExtensions;
using GrowSphere.Core;
using GrowSphere.Domain.Models.UserModel;

namespace GrowSphere.Domain.Models.ProjectModel
{
    public class ProjectMember
    {
        public ProjectId ProjectId { get; private set; }
        public Project Project { get; private set; }

        public UserId UserId { get; private set; }
        public User User { get; private set; }

        public MemberRole Role { get; private set; }

        private ProjectMember() { }

        private ProjectMember(ProjectId projectId, UserId userId, MemberRole role)
        {
            ProjectId = projectId;
            UserId = userId;
            Role = role;
        }

        public static Result<ProjectMember, Error> Create(
            ProjectId projectId,
            UserId userId,
            MemberRole role)
        {
            if (projectId.Value == Guid.Empty)
                return Errors.General.NotFound(projectId.Value);
            
            if (userId.Value == Guid.Empty)
                return Errors.General.NotFound(projectId.Value);
            
            return new ProjectMember(projectId, userId, role);
        }
        
        public void SetProject(Project project)
        {
            Project = project;
        }

        public void SetUser(User user)
        {
            User = user;
        }

        public void ChangeRole(MemberRole newRole)
        {
            if (Role != newRole)
                Role = newRole;
        }
    }

    public enum MemberRole
    {
        Developer,
        Designer,
        Tester,
        ProjectManager,
        BuisnessAnalyst
    }
}
