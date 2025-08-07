using CSharpFunctionalExtensions;
using GrowSphere.Application.DTOs;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.SkillModel;
using GrowSphere.Domain.Models.UserModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrowSphere.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> Add(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return user.Id.Value;
    }

    public async Task<Result<User,Error>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var userId = UserId.Create(id);
        
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
            return Errors.General.NotFound(id);
        
        return user;
    }

    public async Task<Result<User,Error>> GetByEmail(string email)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Value == email);

        if (user == null)
            return Errors.General.NotFound();

        return user;
    }

    public async Task<Result<IEnumerable<User>, Error>> GetAll(CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .Include(u => u.Skills)
                .ThenInclude(s => s.Skill)
            .ToListAsync(cancellationToken);
            
        return users;
    }
}