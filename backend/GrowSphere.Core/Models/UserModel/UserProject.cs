using CSharpFunctionalExtensions;
using GrowSphere.Core;
using GrowSphere.Domain.Models.ProjectModel;

namespace GrowSphere.Domain.Models.UserModel;

public class UserProject
{
    public UserId UserId { get; private set; }
    public User User { get; private set; }
    
    public ProjectId ProjectId { get; private set; }
    public Project Project { get; private set; }

    public UserProject(){}

    private UserProject(UserId userid, ProjectId projectId)
    {
        UserId = userid;
        ProjectId = projectId;
    }

    public static Result<UserProject, Error> Create(UserId userId, ProjectId projectId)
    {
        if(userId.Value == Guid.Empty)
            return Errors.General.ValueIsRequired("userId");
        if(projectId.Value == Guid.Empty)
            return Errors.General.ValueIsRequired("projectId");
        
        return new UserProject(userId, projectId);
    }
}