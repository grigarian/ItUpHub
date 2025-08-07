using CSharpFunctionalExtensions;
using GrowSphere.Application.DTOs;
using GrowSphere.Core;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.SkillModel;
using GrowSphere.Domain.Models.UserModel;

namespace GrowSphere.Application.Interfaces;

public interface IUserRepository
{
    Task<Guid> Add(User user, CancellationToken cancellationToken);
    
    Task<Result<User,Error>> GetById(Guid id, CancellationToken cancellationToken);
    
    Task<Result<User, Error>> GetByEmail(string email);

    Task<Result<IEnumerable<User>, Error>> GetAll(CancellationToken cancellationToken);
}