namespace GrowSphere.Application.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
}