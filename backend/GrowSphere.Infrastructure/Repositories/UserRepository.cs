using CSharpFunctionalExtensions;
using GrowSphere.Application.DTOs;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.SkillModel;
using GrowSphere.Domain.Models.UserModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrowSphere.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> Add(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return user.Id.Value;
    }

    public async Task<Result<User,Error>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var userId = UserId.Create(id);
        
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
            return Errors.General.NotFound(id);
        
        return user;
    }

    public async Task<Result<User,Error>> GetByEmail(string email)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Value == email);

        if (user == null)
            return Errors.General.NotFound();

        return user;
    }

    public async Task<Result<Guid, Error>> RemoveSkill
    (
        UserId userId,
        SkillId skillId,
        [FromServices] ISkillRepository skillRepository,
        CancellationToken cancellationToken
    )
    {
        var userResult = await GetById(userId.Value, cancellationToken);

        if (userResult.IsFailure)
            return userResult.Error;
        
        var user = userResult.Value;
        
        var userSkills = await skillRepository.GetSkillsByUserIdAsync(userId.Value, cancellationToken);

        var skill = userSkills.Value.FirstOrDefault(s => s.Id.Value == skillId.Value);

        if (skill is null)
            return Errors.General.NotFound(skillId);

        var userSkill =  UserSkill.Create(userId, skillId);

        user.RemoveSkill(userSkill.Value);

        _context.UserSkills.Remove(userSkill.Value); // явно удаляем

        await _context.SaveChangesAsync(cancellationToken);

        return userSkill.Value.SkillId.Value;
    }

    public async Task<Result<Guid, Error>> RemoveProject
    (
        UserId userId,
        ProjectId projectId,
        CancellationToken cancellationToken
    )
    {
        var userResult = await GetById(userId.Value, cancellationToken);

        if (userResult.IsFailure)
            return userResult.Error;
        
        var user = userResult.Value;

        var userProject =  user.Projects.FirstOrDefault(p => p.ProjectId == projectId);

        if (userProject is null)
            return Errors.General.NotFound(projectId);

        user.RemoveProject(userProject);

        _context.UserProjects.Remove(userProject); // явно удаляем

        await _context.SaveChangesAsync(cancellationToken);

        return userProject.ProjectId.Value;
    }
}