using CSharpFunctionalExtensions;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain;
using GrowSphere.Domain.Models.Share;
using GrowSphere.Domain.Models.SkillModel;

namespace GrowSphere.Application.Skills;

public class SkillService
{
    private readonly ISkillRepository _skillRepository;

    public SkillService(ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository;
    }

    public async Task<Result<Guid, Error>> Add(
        AddSkillRequest skillRequest,
        CancellationToken cancellationToken
        )
    {
        var skills = await GetAll(cancellationToken);
        
        if(skills.IsFailure)
            return skills.Error;
        
        var exists = skills.Value
            .Any(s => s.Title.Value == skillRequest.title);
        
        if(exists)
            return Errors.General.ValueIsInvalid("Skill title already exists");

        var id = SkillId.NewId();
        
        var title = Title.Create(skillRequest.title);

        if (title.IsFailure)
            return title.Error;

        var skill =  Skill.Create(id, title.Value);
        
        if(skill.IsFailure)
            return skill.Error;
        
        await _skillRepository.AddSkillAsync(skill.Value, cancellationToken);

        return skill.Value.Id.Value;
    }

    public async Task<Result<IEnumerable<Skill>, Error>> GetAll(CancellationToken cancellationToken)
    {
        var skills = await _skillRepository.GetAllSkillsAsync(cancellationToken);
        
        if(skills.IsFailure)
            return skills.Error;

        return skills.Value.ToList();
    }

    public async Task<Result<IEnumerable<Skill>, Error>> GetByUserId
        (
            Guid id,
            CancellationToken cancellationToken
        )
    {
        var skills = await _skillRepository.GetSkillsByUserIdAsync(id, cancellationToken );
        
        if(skills.IsFailure)
            return skills.Error;
        
        return skills.Value.ToList();
    }
    
    
}