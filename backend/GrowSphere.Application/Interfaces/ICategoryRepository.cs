using CSharpFunctionalExtensions;
using GrowSphere.Application.DTOs;
using GrowSphere.Core;
using GrowSphere.Domain.Models.CategoryModel;

namespace GrowSphere.Application.Interfaces;

public interface ICategoryRepository
{
    Task<Guid> Add(Category category, CancellationToken cancellationToken);
    
    Task<Result<Category, Error>> GetById(Guid id, CancellationToken cancellationToken);
    
    Task<Result<IEnumerable<CategoryDto>, Error>> GetAll(CancellationToken cancellationToken);
}