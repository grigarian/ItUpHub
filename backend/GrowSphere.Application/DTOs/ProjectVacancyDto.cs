namespace GrowSphere.Application.DTOs;

public record ProjectVacancyDto(
    Guid Id,
    Guid ProjectId,
    string Title,
    string Description,
    DateTime CreatedAt,
    IReadOnlyList<SkillDto> Skills,
    IReadOnlyList<VacancyApplicationDto> Applications
);

public record VacancyApplicationDto(
    Guid Id,
    Guid ProjectVacancyId,
    Guid UserId,
    string UserName,
    string Message,
    DateTime CreatedAt,
    string Status,
    string? ManagerComment
);

public record CreateVacancyRequest(string Title, string Description, List<Guid> SkillIds);

public record ApplyToVacancyRequest(string Message);

public record ApproveApplicationRequest(string Message, string Role);

public record RejectApplicationRequest(string Message);