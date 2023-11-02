using Microsoft.AspNetCore.Identity;

namespace CleanArchitectureTemplate.Domain.Entities;

public class ApplicationUserToken : IdentityUserToken<string>
{
    public virtual ApplicationUser User { get; set; } = null!;
}
