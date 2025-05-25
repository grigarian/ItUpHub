using CSharpFunctionalExtensions;
using GrowSphere.Application.DTOs;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain.Models.CategoryModel;
using GrowSphere.Domain.Models.Share;

namespace GrowSphere.Application.Categories;

public class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(
        ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<Guid, Error>> Create(
        CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var id = CategoryId.NewId();
        
        var title = Title.Create(request.Title);

        if (title.IsFailure)
            return title.Error;
        
        var category = Category.Create(id, title.Value);
        
        if (category.IsFailure)
            return category.Error;
        
        await _categoryRepository.Add(category.Value, cancellationToken);
        
        return id.Value;
    }

    public async Task<Result<IEnumerable<CategoryDto>, Error>> GetAll(CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAll(cancellationToken);

        return categories.Value.ToList();
    }
}