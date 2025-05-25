using GrowSphere.Domain.Models.IssueModel;

namespace GrowSphere.Application.Interfaces;

public interface IIssueRepository
{
    Task<Guid> Add(Issue issue, CancellationToken cancellationToken);
}