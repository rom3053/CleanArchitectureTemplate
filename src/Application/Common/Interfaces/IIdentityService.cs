using CleanArchitectureTemplate.Application.Common.Dtos;
using CleanArchitectureTemplate.Domain.Entities;

namespace CleanArchitectureTemplate.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);

    Task<Result> CreateUserAsync(ApplicationUser applicationUser);

    Task<Result> AddRoleAsync(ApplicationUser user, string roleName);
}
