using CSharpFunctionalExtensions;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain;
using GrowSphere.Domain.Models.Share;
using GrowSphere.Domain.Models.SkillModel;
using Microsoft.EntityFrameworkCore;

namespace GrowSphere.Infrastructure.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SkillRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Skill, Error>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var skill = await _dbContext.Skills
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken: cancellationToken);

        if (skill == null)
            return Errors.General.NotFound();

        return skill;
    }

    public async Task<Guid> AddSkillAsync(Skill skill, CancellationToken cancellationToken)
    {
        await _dbContext.Skills.AddAsync(skill, cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return skill.Id.Value;
    }

    public async Task<Result<IEnumerable<Skill>, Error>> GetAllSkillsAsync(CancellationToken cancellationToken)
    {
        var skills = await _dbContext.Skills.ToListAsync(cancellationToken);
        
        return skills;
    }

    public async Task<Result<IEnumerable<Skill>, Error>> GetSkillsByUserIdAsync
        (
        Guid userId,
        CancellationToken cancellationToken
        )
    {
        if(userId.Equals(Guid.Empty))
            return Errors.General.NotFound(userId);
        
        var skills = await _dbContext.UserSkills
            .Where(us => us.UserId == userId)
            .Include(us => us.Skill)
            .Select(us => us.Skill)
            .ToListAsync(cancellationToken: cancellationToken);
        
        return skills;
    }
}