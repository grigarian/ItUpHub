using CSharpFunctionalExtensions;
using GrowSphere.Core;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.Share;
using GrowSphere.Domain.Models.UserModel;

namespace GrowSphere.Domain.Models.IssueModel
{
    public class Issue : Entity<IssueId>
    {
        private Issue(IssueId id) : base(id) { }

        public Title Title { get; private set; } = default!;

        public Description Description { get; private set; } = default!;
        
        public ProjectId ProjectId { get; private set; } = default!;
        public Project Project { get; private set; }

        public UserId AssignedUserId { get; private set; } 
        public User AssignedUser { get; private set; }
        
        public UserId? AssignerUserId { get; private set; }
        public User? AssignerUser { get; private set; }

        public Picture Picture { get; private set; }

        public DateTime CompleteDate { get; private set; }

        public IssueStatus Status { get; private set; } = IssueStatus.Backlog;
        
        public int Order { get; private set; }

        private Issue(
            IssueId issueId,
            Title title,
            Description description,
            UserId? assignedUserId,
            UserId? assignerUserId,
            DateTime completeDate,
            IssueStatus status,
            Picture? picture,
            int order)
            : base(issueId)
        {
            Title = title;
            Description = description;
            AssignedUserId = assignedUserId;
            AssignerUserId = assignerUserId;
            CompleteDate = completeDate;
            Status = status;
            Picture = picture;
            Order = order;
        }

        public static Result<Issue, Error> Create(IssueId id,
            Title title,
            Description description,
            DateTime completeDate,
            IssueStatus issueStatus,
            UserId? assignedUserId = null,
            UserId? assignerUserId = null,
            Picture? picture = null,
            int order = 0)
        {
            /*if (completeDate <= DateTime.Now)
                return Errors.General.ValueIsInvalid("completeDate");*/

            return new Issue(id,
                title,
                description,
                assignedUserId,
                assignerUserId,
                completeDate,
                issueStatus,
                picture,
                order);
        }
        
        public Result<Result, Error> Update(
            Title title,
            Description description,
            DateTime completeDate,
            UserId assignedUserId)
        {

            Title = title;
            Description = description;
            CompleteDate = completeDate;
            AssignedUserId = assignedUserId;
            return Result.Success();
        }
        
        public Result<Result, Error> AssignToUser(User user)
        {
            if (user.Id == null)
                return Errors.General.NotFound(user.Id.Value);

            AssignedUserId = user.Id;
            AssignedUser = user;
            return Result.Success();
        }

        public Result<Result, Error> SetProject(Project project)
        {
            if (project.Id == null)
                return Errors.General.NotFound(project.Id.Value);
            
            Project = project;
            ProjectId = project.Id;

            return Result.Success();
        }

        public Result UpdateStatus(IssueStatus newStatus)
        {
            Status = newStatus;
            return Result.Success();
        }
        
        public Result<Result, Error> UpdateOrder(int newOrder)
        {
            if (newOrder < 0)
                return Errors.General.ValueIsInvalid("order");

            Order = newOrder;
            return Result.Success();
        }
        
    }
}
