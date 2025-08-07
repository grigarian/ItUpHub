using CSharpFunctionalExtensions;
using GrowSphere.Application.Interfaces;
using GrowSphere.Domain.Models.ProjectVacancyModel;
using Microsoft.EntityFrameworkCore;

namespace GrowSphere.Infrastructure.Repositories;

public class ProjectVacancyRepository : IProjectVacancyRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectVacancyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectVacancy?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProjectVacancies
            .Include(v => v.Skills)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<ProjectVacancy?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProjectVacancies
            .Include(v => v.Skills)
            .Include(v => v.Project)
            .Include(v => v.VacancyApplications)
                .ThenInclude(a => a.User)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<Result> AddAsync(ProjectVacancy vacancy, CancellationToken cancellationToken = default)
    {
        await _context.ProjectVacancies.AddAsync(vacancy, cancellationToken);
        return Result.Success();
    }

    public async Task<List<ProjectVacancy>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var vacancies = await _context.ProjectVacancies
            .Include(v => v.Skills)
            .Include(v => v.Project)
            .Include(v => v.VacancyApplications)
            .ToListAsync(cancellationToken);
        
        return vacancies;
    }

    public async Task<List<ProjectVacancy>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return await _context.ProjectVacancies
            .Include(v => v.Skills)
            .Include(v => v.Project)
            .Include(v => v.VacancyApplications)
            .Where(v => v.ProjectId == projectId)
            .ToListAsync(cancellationToken);
    }
}