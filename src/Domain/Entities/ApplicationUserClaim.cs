using Microsoft.AspNetCore.Identity;

namespace CleanArchitectureTemplate.Domain.Entities;

public class ApplicationUserClaim : IdentityUserClaim<string>
{
    public virtual ApplicationUser User { get; set; } = null!;
}
