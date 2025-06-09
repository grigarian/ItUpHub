using CSharpFunctionalExtensions;
using GrowSphere.Domain.Models.ProjectVacancyModel;

namespace GrowSphere.Application.Interfaces;

public interface IVacancyApplicationRepository
{
    Task<VacancyApplication?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> AddAsync(VacancyApplication application, CancellationToken cancellationToken = default);
    Task<List<VacancyApplication>> GetByVacancyIdAsync(Guid vacancyId, CancellationToken cancellationToken = default);
}