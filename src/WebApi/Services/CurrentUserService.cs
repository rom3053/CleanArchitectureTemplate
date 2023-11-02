﻿using System.Security.Claims;
using CleanArchitectureTemplate.Application.Common.Interfaces;

namespace CleanArchitectureTemplate.WebApi.Services;

/// <summary>
/// Service to receive information about user who is doing requests to api
/// </summary>
public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => GetStringUserClaim(ClaimTypes.NameIdentifier);

    public string? Email => GetStringUserClaim(ClaimTypes.Email);

    public List<string>? Roles => _httpContextAccessor.HttpContext?.User?.Claims.Where(x => x.Type == ClaimTypes.Role)?.Select(x => x.Value).ToList();

    /// <summary>
    /// Method for receiving value of claim by its type
    /// </summary>
    /// <param name="claimType">Claim type</param>
    /// <returns>Value of claim by type</returns>
    private string GetStringUserClaim(string claimType)
    {
        return _httpContextAccessor.HttpContext?.User.Claims.SingleOrDefault(c => c.Type == claimType)?.Value!;
    }
}
