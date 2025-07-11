using GrowSphere.Application.DTOs;
using GrowSphere.Domain.Models.ProjectVacancyModel;

namespace GrowSphere.Application.Mappers;

public static class VacancyApplicationMapper
{
    public static VacancyApplicationDto ToVacancyApplicationDto(VacancyApplication application, string userName = "", string? managerComment = null)
        => new VacancyApplicationDto(
            application.Id,
            application.ProjectVacancyId,
            application.UserId.Value,
            userName,
            application.Message,
            application.CreatedAt.ToUniversalTime(),
            application.Status.ToString(),
            managerComment
        );
} 