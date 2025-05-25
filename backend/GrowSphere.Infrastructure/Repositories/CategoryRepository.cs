using CSharpFunctionalExtensions;
using GrowSphere.Application.DTOs;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain;
using GrowSphere.Domain.Models.CategoryModel;
using Microsoft.EntityFrameworkCore;

namespace GrowSphere.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> Add(Category category, CancellationToken cancellationToken)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return category.Id.Value;
    }

    public async Task<Result<Category, Error>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var categoryId = CategoryId.Create(id);
        
        var category = await _context.Categories
            .FirstOrDefaultAsync(u => u.Id == categoryId, cancellationToken);

        if (category == null)
            return Errors.General.NotFound(id);
        
        return category;
    }

    public async Task<Result<IEnumerable<CategoryDto>, Error>> GetAll(CancellationToken cancellationToken)
    {
        var categories = await _context.Categories
            .Select(c => new CategoryDto(
                c.Id.Value,
                c.Title.Value
                ))
            .ToListAsync(cancellationToken);
        
        return categories; 
    }
}