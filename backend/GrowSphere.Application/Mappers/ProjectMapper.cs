using GrowSphere.Application.DTOs;
using GrowSphere.Domain.Models.ProjectModel;
using System.Collections.Generic;
using System.Linq;

namespace GrowSphere.Application.Mappers;

public static class ProjectMapper
{
    public static ProjectDto ToProjectDto(Project project, string categoryName, IEnumerable<ProjectMember> members)
        => new ProjectDto
        {
            Id = project.Id.Value,
            Title = project.Title.Value,
            Description = project.Description.Value,
            Status = project.Status.Value,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            CreationDate = project.CreationDate,
            CategoryName = categoryName,
            Members = members.Select(ProjectMemberMapper.ToProjectMemberDto).ToList()
        };

    public static ProjectListItemDto ToProjectListItemDto(Project project) =>
        new ProjectListItemDto
        (project.Id.Value, project.Title.Value, project.Description.Value, project.Category.Title.Value);
            
    public static ProjectWithCategoryDto ToProjectWithCategoryDto(Project project) =>
        new ProjectWithCategoryDto(
            project.Id.Value,
            project.Title.Value,
            project.Description.Value,
            project.Category?.Title?.Value,
            project.Status.Value,
            project.StartDate,
            project.EndDate,
            project.CreationDate
        );
        
} 