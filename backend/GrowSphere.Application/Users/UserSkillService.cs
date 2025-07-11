using CSharpFunctionalExtensions;
using GrowSphere.Application.DTOs;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain;
using GrowSphere.Domain.Models.SkillModel;
using GrowSphere.Domain.Models.UserModel;
using Microsoft.AspNetCore.Mvc;

namespace GrowSphere.Application.Users;

public class UserSkillService
{
    private readonly IUserRepository _userRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserSkillService(
        IUserRepository userRepository,
        ISkillRepository skillRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _skillRepository = skillRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Skill, Error>> AddSkill(
        Guid userId,
        AddSkillToUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(userId, cancellationToken);
        if (user.IsFailure)
            return user.Error;
        var skill = await _skillRepository.GetById(request.SkillId, cancellationToken);
        if(skill.IsFailure)
            return skill.Error;
        var userSkills = await _skillRepository.GetSkillsByUserIdAsync(userId, cancellationToken);
        if (userSkills.Value.Any(s => s.Id == request.SkillId))
        {
            return Errors.General.ValueIsInvalid("Skill Id already exists");
        }
        user.Value.AddSkill(skill.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return skill.Value;
    }

    public async Task<Result<Guid, Error>> RemoveSkill(
        Guid userId,
        Guid skillId,
        [FromServices] ISkillRepository skillRepository,
        CancellationToken cancellationToken)
    {
        var userIdObj = UserId.Create(userId);
        var skillIdObj = SkillId.Create(skillId);
        if (userIdObj.Value == Guid.Empty || skillIdObj.Value == Guid.Empty)
            return Errors.General.NotFound();
        return await _userRepository.RemoveSkill(userIdObj, skillIdObj, skillRepository, cancellationToken);
    }
} 