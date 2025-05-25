namespace GrowSphere.Application.DTOs;

public record ProjectDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string Status { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public DateTime CreationDate { get; init; }
    public string CategoryName { get; init; }
    public List<ProjectMemberDto> Members { get; init; } = [];
}