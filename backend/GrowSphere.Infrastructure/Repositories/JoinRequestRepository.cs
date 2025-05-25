using GrowSphere.Application.Interfaces;
using GrowSphere.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GrowSphere.Infrastructure.Repositories;

public class JoinRequestRepository: IJoinRequestRepository
{
    private readonly ApplicationDbContext _context;

    public JoinRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(JoinRequest request, CancellationToken cancellationToken)
    {
        await _context.JoinRequests.AddAsync(request, cancellationToken);
    }

    public async Task<List<JoinRequest>> GetByProjectId(Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.JoinRequests
            .Where(jr => jr.ProjectId == projectId)
            .ToListAsync(cancellationToken);
    }

    public async Task<JoinRequest?> GetById(Guid requestId, CancellationToken cancellationToken)
    {
        return await _context.JoinRequests
            .FirstOrDefaultAsync(jr => jr.Id == requestId, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid projectId, Guid userId, CancellationToken cancellationToken)
    {
        return await _context.JoinRequests
            .AnyAsync(jr => jr.ProjectId == projectId && jr.UserId == userId, cancellationToken);
    }

    public async Task UpdateAsync(JoinRequest request, CancellationToken cancellationToken)
    {
         _context.JoinRequests.Update(request);
    }
}