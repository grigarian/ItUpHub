namespace GrowSphere.Application.DTOs;

public record ProjectListItemDto(Guid Id, string Title, string Description, string Category);

public record ProjectWithCategoryDto(Guid Id, string Title, string Description, string? CategoryName, string Status, DateTime StartDate, DateTime? EndDate, DateTime CreationDate);