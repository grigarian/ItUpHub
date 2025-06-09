using CSharpFunctionalExtensions;
using GrowSphere.Core;
using GrowSphere.Domain.Models.IssueModel;

namespace GrowSphere.Application.Interfaces;

public interface IIssueRepository
{
    Task<Guid> AddAsync(Issue issue, CancellationToken cancellationToken);
    
    Task<Issue?> GetByIdAsync(IssueId id, CancellationToken cancellationToken = default);
    
    Task<Issue?> GetByIdWithProjectAsync(IssueId id, CancellationToken cancellationToken);
    
    Task<List<Issue>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken);
    
    void Update(Issue issue);
    void Remove(Issue issue);
}