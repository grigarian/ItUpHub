using CSharpFunctionalExtensions;
using GrowSphere.Core;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.Share;
using GrowSphere.Domain.Models.SkillModel;

namespace GrowSphere.Domain.Models.UserModel
{
    public class User : Entity<UserId>
    {
        public const int USERNAME_MAX_LENGTH = 50;
        
        private readonly List<UserProject> _projects = [];
        
        private readonly List<UserSkill> _skills = [];

        private User(UserId id) : base(id) { }

        public UserName Name { get; private set; }

        public Email Email { get; private set; }

        public string PasswordHash { get; private set; }

        public Picture ProfilePicture { get; private set; }

        public Bio Bio { get; private set; }

        public UserStatus UserStatus { get; private set; } = UserModel.UserStatus.Online;

        public DateTime CreatedDate { get; private set; } = DateTime.Now;

        public IReadOnlyCollection<UserProject> Projects => _projects.AsReadOnly();
        
        public IReadOnlyCollection<UserSkill> Skills => _skills.AsReadOnly();
        
        public void UpdateProfilePicture(Picture picture) => ProfilePicture = picture;
        
        public void UpdateBio(Bio bio) => Bio = bio;

        public void AddSkills(IEnumerable<Skill> skills)
        {
            foreach (var skill in skills)
                AddSkill(skill);
        }
        
        public void AddSkill(Skill skill)
        { 
            if (_skills.Any(us => us.SkillId == skill.Id))
                return;

            var userSkill = UserSkill.Create(this.Id, skill.Id);
            _skills.Add(userSkill.Value);
        }

        public void RemoveSkill(UserSkill skill)
        {
            if(!_skills.Contains(skill)) 
                return;
            
            _skills.Remove(skill);
        }

        public void AddProject(Project project)
        { 
            if (_projects.Any(up => up.ProjectId == project.Id))
                return;

            var userProject = UserProject.Create(this.Id, project.Id);
            _projects.Add(userProject.Value);
        }

        public void RemoveProject(UserProject userProject)
        {
            if(!_projects.Contains(userProject))
                return;
            
            _projects.Remove(userProject);
        }

        private User(
            UserId id,
            UserName name,
            Email email,
            string password,
            DateTime createDate,
            Bio? bio,
            Picture? profilePicture
            ) : base(id)
        {
            Id = id;
            Name = name;
            Email = email;
            PasswordHash = password;
            CreatedDate = createDate;
            Bio = bio;
            ProfilePicture = profilePicture;
        }

        public static Result<User, Error> Register(
            UserId id,
            UserName name,
            Email email,
            string password,
            DateTime createDate,
            Bio? bio,
            Picture? profilePicture)
        {
            if (string.IsNullOrWhiteSpace(password))
                return Errors.General.ValueIsRequired("password");

            return new User(id, name, email, password, createDate, bio, profilePicture);
        }

    }
}
