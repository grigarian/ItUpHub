using CSharpFunctionalExtensions;
using GrowSphere.Core;
using GrowSphere.Domain.Models.SkillModel;

namespace GrowSphere.Application.Interfaces;

public interface ISkillRepository
{
    Task<Result<Skill, Error>> GetById(Guid id, CancellationToken cancellationToken);
    Task<Guid> AddSkillAsync(Skill skill, CancellationToken cancellationToken);
    Task<Result<IEnumerable<Skill>, Error>> GetAllSkillsAsync(CancellationToken cancellationToken);
    Task<Result<IEnumerable<Skill>, Error>> GetSkillsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}