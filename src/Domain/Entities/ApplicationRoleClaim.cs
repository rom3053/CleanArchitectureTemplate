using Microsoft.AspNetCore.Identity;

namespace CleanArchitectureTemplate.Domain.Entities;

public class ApplicationRoleClaim : IdentityRoleClaim<string>
{
    public virtual ApplicationRole Role { get; set; } = null!;
}
