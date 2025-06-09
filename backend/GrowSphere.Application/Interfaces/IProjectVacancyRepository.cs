using CSharpFunctionalExtensions;
using GrowSphere.Domain.Models.ProjectVacancyModel;

namespace GrowSphere.Application.Interfaces;

public interface IProjectVacancyRepository
{
    Task<ProjectVacancy?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> AddAsync(ProjectVacancy vacancy, CancellationToken cancellationToken = default);
    
    Task<List<ProjectVacancy>> GetAllAsync(CancellationToken cancellationToken = default);
}