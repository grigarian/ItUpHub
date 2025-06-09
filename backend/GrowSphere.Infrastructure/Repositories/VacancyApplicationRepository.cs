using CSharpFunctionalExtensions;
using GrowSphere.Application.Interfaces;
using GrowSphere.Domain.Models.ProjectVacancyModel;
using Microsoft.EntityFrameworkCore;

namespace GrowSphere.Infrastructure.Repositories;

public class VacancyApplicationRepository : IVacancyApplicationRepository
{
    private readonly ApplicationDbContext _context;

    public VacancyApplicationRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<VacancyApplication?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.VacancyApplications.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Result> AddAsync(VacancyApplication application, CancellationToken cancellationToken = default)
    {
        await _context.VacancyApplications.AddAsync(application, cancellationToken);
        return Result.Success();
    }

    public async Task<List<VacancyApplication>> GetByVacancyIdAsync(Guid vacancyId, CancellationToken cancellationToken = default)
    {
        return await _context.VacancyApplications
            .Where(a => a.ProjectVacancyId == vacancyId)
            .ToListAsync(cancellationToken);
    }
}