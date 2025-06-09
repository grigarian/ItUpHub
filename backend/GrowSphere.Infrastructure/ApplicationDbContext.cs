using GrowSphere.Domain.Models;
using GrowSphere.Domain.Models.CategoryModel;
using GrowSphere.Domain.Models.IssueModel;
using GrowSphere.Domain.Models.NotificationModel;
using GrowSphere.Domain.Models.ProjectMessage;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.ProjectVacancyModel;
using GrowSphere.Domain.Models.SkillModel;
using GrowSphere.Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GrowSphere.Infrastructure;

public class ApplicationDbContext(IConfiguration configuration) :DbContext
{
    private const string DATABASE = "Database";
    
    public DbSet<User> Users => Set<User>();
    
    public DbSet<Issue> Issues => Set<Issue>();
    
    public DbSet<Project> Projects => Set<Project>();
    
    public DbSet<Skill> Skills => Set<Skill>();
    
    public DbSet<Category> Categories => Set<Category>();
    
    public DbSet<UserSkill> UserSkills => Set<UserSkill>();

    public DbSet<UserProject> UserProjects => Set<UserProject>();
    
    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
    
    public DbSet<JoinRequest> JoinRequests => Set<JoinRequest>();
    
    public DbSet<Notification> Notifications => Set<Notification>();
    
    public DbSet<ProjectMessage> ProjectMessages => Set<ProjectMessage>();
    
    public DbSet<VacancyApplication> VacancyApplications => Set<VacancyApplication>();
    
    public DbSet<ProjectVacancy> ProjectVacancies => Set<ProjectVacancy>();
    
    public DbSet<ProjectVacancySkill> ProjectVacancySkills => Set<ProjectVacancySkill>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE));
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.UseCamelCaseNamingConvention();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
    
    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}