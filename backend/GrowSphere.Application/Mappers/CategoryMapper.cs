using GrowSphere.Application.DTOs;
using GrowSphere.Domain.Models.CategoryModel;

namespace GrowSphere.Application.Mappers;

public static class CategoryMapper
{
    public static CategoryDto ToCategoryDto(Category category)
        => new(category.Id.Value, category.Title.Value);
} 