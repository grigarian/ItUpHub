using CSharpFunctionalExtensions;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain.Models.IssueModel;
using Microsoft.EntityFrameworkCore;

namespace GrowSphere.Infrastructure.Repositories;

public class IssueRepository: IIssueRepository
{
    private readonly ApplicationDbContext _context;
    
    public IssueRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> AddAsync(Issue issue, CancellationToken cancellationToken)
    {
        await _context.Issues.AddAsync(issue, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return issue.Id.Value;
    }

    public async Task<Issue?> GetByIdAsync(IssueId id, CancellationToken cancellationToken = default)
    {
        return await _context.Issues
            .Include(i => i.AssignerUser)
            .Include(i => i.AssignedUser)
            .Include(i => i.Project)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<Issue?> GetByIdWithProjectAsync(IssueId id, CancellationToken cancellationToken)
    {
        return await _context.Issues
            .Include(i => i.Project)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<List<Issue>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.Issues
            .Where(i => i.ProjectId == projectId)
            .OrderBy(i => i.Order)
            .ToListAsync(cancellationToken);
    }

    public void Update(Issue issue)
    {
        _context.Issues.Update(issue);
    }

    public void Remove(Issue issue)
    {
        _context.Issues.Remove(issue);
    }
}