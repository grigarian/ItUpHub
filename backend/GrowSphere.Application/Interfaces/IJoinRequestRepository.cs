using GrowSphere.Domain.Models;

namespace GrowSphere.Application.Interfaces;

public interface IJoinRequestRepository
{
        Task AddAsync(JoinRequest request, CancellationToken cancellationToken);
        Task<List<JoinRequest>> GetByProjectId(Guid projectId, CancellationToken cancellationToken);
        Task<JoinRequest?> GetById(Guid requestId, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid projectId, Guid userId, CancellationToken cancellationToken);
        Task UpdateAsync(JoinRequest request, CancellationToken cancellationToken);
}