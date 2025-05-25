using CSharpFunctionalExtensions;
using GrowSphere.Core;
using GrowSphere.Domain.Models.CategoryModel;
using GrowSphere.Domain.Models.IssueModel;
using GrowSphere.Domain.Models.Share;
using GrowSphere.Domain.Models.UserModel;

namespace GrowSphere.Domain.Models.ProjectModel
{
    public class Project : Entity<ProjectId>
    {
        private readonly List<Issue> _issues = [];

        private readonly List<ProjectMember> _members = [];
        
        private Project(ProjectId id) : base(id) { }

        public Title Title { get; private set; }

        public Description Description { get; private set; }
        
        public CategoryId CategoryId { get; private set; }
        
        public Category Category { get; private set; }

        public ProjectStatus Status { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime? EndDate { get; private set; }

        public DateTime UpdateDate { get; private set; }

        public DateTime CreationDate { get; private set; } = DateTime.UtcNow;

        public IReadOnlyCollection<Issue> Issues => _issues;

        public IReadOnlyCollection<ProjectMember> Members => _members.AsReadOnly();
        
        public void AddIssue(Issue issue) => _issues.Add(issue);
        
        public void AddCategory(Category category)
        {
            CategoryId = category.Id;
            Category = category;
        }

        private Project(
            ProjectId id,
            Title title,
            Description description,
            ProjectStatus status,
            DateTime startDate,
            DateTime? endDate,
            DateTime updateDate,
            DateTime creationDate) 
            : base(id)
        {
            Title = title;
            Description = description;
            Status = status;
            StartDate = startDate;
            EndDate = endDate;
            UpdateDate = updateDate;
            CreationDate = creationDate;
        }

        public static Result<Project, Error> Create(
            ProjectId id,
            Title title,
            Description description,
            ProjectStatus status,
            DateTime startDate,
            DateTime? endDate,
            DateTime updateDate,
            DateTime creationDate)
        {
            // if (endDate <= DateTime.Now) 
            //     return Errors.General.ValueIsInvalid("end_date");
            //
            // if(startDate <  DateTime.Now)
            //     return Errors.General.ValueIsInvalid("start_date");
            //
            // if(updateDate < DateTime.Now)
            //     return Errors.General.ValueIsInvalid("updated_at");
            

            return new Project(id,
                title,
                description,
                status,
                startDate,
                endDate,
                updateDate,
                creationDate);
        }

        public void AddMember(ProjectMember member)
        {
            _members.Add(member);
        }

        public Result<Result, Error> ChangeTitle(string title)
        {
            var newTitle = Title.Create(title);
            if (newTitle.IsFailure)
                return newTitle.Error;
            if (Title == newTitle.Value)
                return Errors.General.ValueIsInvalid("title");
            
            Title = newTitle.Value;
            UpdateDate = DateTime.UtcNow;
            return Result.Success();
        }

        public Result<Result, Error> ChangeDescription(string description)
        {
            var newDescription = Description.Create(description);
            if(newDescription.IsFailure)
                return newDescription.Error;
            if(Description == newDescription.Value)
                return Errors.General.ValueIsInvalid("description");
            
            Description = newDescription.Value;
            UpdateDate = DateTime.UtcNow;
            return Result.Success();
        }

        public Result<Result, Error> ChangeEndDate(DateTime? endDate)
        {
            var newEnd = endDate?.ToUniversalTime();
            
            // Добавить проверку на то, что даты не меньше текущей
            
            EndDate = endDate;
            UpdateDate = DateTime.UtcNow;
            return Result.Success();
        }

        public Result<Result, Error> ChangeStatus(ProjectStatus status)
        {
            if(Status == status)
                return Errors.General.ValueIsInvalid("status");
            
            Status = status;
            UpdateDate = DateTime.UtcNow;
            return Result.Success();
        }

        public Result<Result, Error> ChangeCategory(Category category)
        {
            var newCategoryId = category.Id;
            
            if(Category == category)
                return Errors.General.ValueIsInvalid("category");
            if(CategoryId == newCategoryId)
                return Errors.General.ValueIsInvalid("categoryId");
            
            CategoryId = newCategoryId;
            Category = category;
            UpdateDate = DateTime.UtcNow;
            return Result.Success();
        }

        public bool IsManagedBy(UserId userId)
        {
            var members = Members
                .Any(m => m.UserId == userId && m.Role == MemberRole.ProjectManager);

            return members;
        }
    }

}
