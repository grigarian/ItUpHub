namespace GrowSphere.Application.Issues;

public record CreateIssueRequest(
    string Title,
    string Description,
    Guid ProjectId,
    DateTime CompleteDate,
    Guid AssignerUserId,
    Guid AssignedUserId);
    
public class AssignUserRequest
{
    public Guid UserId { get; set; }
}

public class ChangeStatusRequest
{
    public string Status { get; set; } = default!;
}

public class ReorderIssuesRequest
{
    public List<IssueColumn> Columns { get; set; } = new();
}

public class IssueColumn
{
    public string Status { get; set; } = default!;
    public List<Guid> IssueIds { get; set; } = new();
}

public record UpdateIssueRequest
{
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public Guid AssignedUserId { get; init; }
    public DateTime CompleteDate { get; init; }
}