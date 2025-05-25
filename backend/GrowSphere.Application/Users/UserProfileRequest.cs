using GrowSphere.Domain.Models.Share;
using Microsoft.AspNetCore.Http;

namespace GrowSphere.Application.Users;

public record UserProfileRequest(string Bio);