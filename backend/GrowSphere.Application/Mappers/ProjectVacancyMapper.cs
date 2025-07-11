using GrowSphere.Application.DTOs;
using GrowSphere.Domain.Models.ProjectVacancyModel;
using System.Collections.Generic;
using System.Linq;
using GrowSphere.Domain.Models.SkillModel;
using GrowSphere.Application.Mappers;

namespace GrowSphere.Application.Mappers;

public static class ProjectVacancyMapper
{
    public static ProjectVacancyDto ToProjectVacancyDto(ProjectVacancy vacancy, IEnumerable<Skill> skills, IEnumerable<VacancyApplication> applications)
        => new ProjectVacancyDto(
            vacancy.Id,
            vacancy.ProjectId,
            vacancy.Title.Value,
            vacancy.Description.Value,
            vacancy.CreatedAt.ToUniversalTime(),
            skills.Select((Skill s) => SkillMapper.ToSkillDto(s)).ToList(),
            applications.Select((VacancyApplication a) => VacancyApplicationMapper.ToVacancyApplicationDto(a)).ToList()
        );
} 