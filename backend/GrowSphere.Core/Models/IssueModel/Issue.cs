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

        public UserId? AssignerUserId { get; private set; } = null;

        public Picture Picture { get; private set; } 

        public DateTime CompleteDate { get; private set; }

        public IssueStatus Status { get; private set; } = IssueStatus.Wait;
        
        public Project Project { get; private set; }
        
        public User AssignerUser { get; private set; }

        private Issue(
            IssueId issueId,
            Title title,
            Description description,
            ProjectId projectId,
            UserId assignerUserId,
            Picture picture,
            DateTime completeDate,
            IssueStatus status)
            : base(issueId)
        {
            Title = title;
            Description = description;
            ProjectId = projectId;
            AssignerUserId = assignerUserId;
            Picture = picture;
            CompleteDate = completeDate;
            Status = status;
        }

        public static Result<Issue, Error> Create(IssueId id,
            Title title,
            Description description,
            ProjectId projectId,
            Picture picture,
            DateTime completeDate,
            IssueStatus issueStatus,
            UserId assignerUserId = null)
        {
            if (completeDate <= DateTime.Now)
                return Errors.General.ValueIsInvalid("completeDate");

            return new Issue(id,
                title,
                description,
                projectId,
                assignerUserId,
                picture,
                completeDate,
                issueStatus);
        }

        
    }
}
