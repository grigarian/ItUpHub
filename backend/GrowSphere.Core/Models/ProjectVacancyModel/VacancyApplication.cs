using CSharpFunctionalExtensions;
using GrowSphere.Core;
using GrowSphere.Domain.Models.UserModel;

namespace GrowSphere.Domain.Models.ProjectVacancyModel;

public class VacancyApplication
{
    private VacancyApplication() { }

    public Guid Id { get; private set; }
    public Guid ProjectVacancyId { get; private set; }
    public UserId UserId { get; private set; }
    public string Message { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public VacancyApplicationStatus Status { get; private set; }
    public string? ManagerComment { get; private set; }
    public User User { get; private set; }
    public ProjectVacancy ProjectVacancy { get; private set; }

    public static Result<VacancyApplication, Error> Create(Guid vacancyId, Guid userId, string message)
    {
        if (vacancyId == Guid.Empty)
            return Errors.General.ValueIsRequired("vacancyId");

        if (userId == Guid.Empty)
            return Errors.General.ValueIsRequired("userId");

        if (string.IsNullOrWhiteSpace(message))
            return Errors.General.ValueIsRequired("message");

        return new VacancyApplication
        {
            Id = Guid.NewGuid(),
            ProjectVacancyId = vacancyId,
            UserId = UserId.Create(userId),
            Message = message.Trim(),
            CreatedAt = DateTime.UtcNow,
            Status = VacancyApplicationStatus.Pending
        };
    }
    
    public Result Approve(string? comment = null)
    {
        if (Status != VacancyApplicationStatus.Pending)
            return Result.Failure("Application is not pending");

        Status = VacancyApplicationStatus.Approved;
        ManagerComment = comment;
        return Result.Success();
    }

    public Result Reject(string? comment = null)
    {
        if (Status != VacancyApplicationStatus.Pending)
            return Result.Failure("Application is not pending");

        Status = VacancyApplicationStatus.Rejected;
        ManagerComment = comment;
        return Result.Success();
    }
}

public enum VacancyApplicationStatus
{
    Pending,
    Approved,
    Rejected
}