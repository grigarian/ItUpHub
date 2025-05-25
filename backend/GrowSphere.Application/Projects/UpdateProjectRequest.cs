using GrowSphere.Domain.Models.ProjectModel;

namespace GrowSphere.Application.Projects;

public record UpdateProjectRequest(
    string? Title,
    string? Description,
    string? ProjectStatus,
    DateTime? EndDate,
    Guid? CategoryId
    );