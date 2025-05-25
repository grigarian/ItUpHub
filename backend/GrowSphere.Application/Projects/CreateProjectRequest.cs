using GrowSphere.Domain.Models.CategoryModel;

namespace GrowSphere.Application.Projects;

public record CreateProjectRequest(
    string Title,
    string Description,
    DateTime StartDate,
    DateTime? EndDate,
    Guid CategoryId
    );