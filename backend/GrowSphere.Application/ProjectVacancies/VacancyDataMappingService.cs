 using GrowSphere.Application.DTOs;
using GrowSphere.Domain.Models.ProjectVacancyModel;
using GrowSphere.Application.Mappers;

namespace GrowSphere.Application.ProjectVacancies;

public class VacancyDataMappingService
{
    public List<VacancyApplicationDto> MapPendingApplicationsToDto(IEnumerable<VacancyApplication> applications)
    {
        return applications
            .Where(a => a.Status == VacancyApplicationStatus.Pending)
            .Select(a => VacancyApplicationMapper.ToVacancyApplicationDto(
                a,
                a.User?.Name.Value ?? string.Empty,
                a.ManagerComment))
            .ToList();
    }

    public List<ProjectVacancyDto> MapVacanciesToDto(IEnumerable<ProjectVacancy> vacancies)
    {
        return vacancies
            .Select(vacancy => ProjectVacancyMapper.ToProjectVacancyDto(
                vacancy,
                vacancy.Skills.ToList(),
                vacancy.VacancyApplications
                    .Where(a => a.Status == VacancyApplicationStatus.Pending)
                    .ToList()
            ))
            .ToList();
    }
}