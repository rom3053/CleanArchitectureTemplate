﻿using Microsoft.AspNetCore.Identity;

namespace CleanArchitectureTemplate.Domain.Entities;

public class ApplicationUserLogin : IdentityUserLogin<string>
{
    public virtual ApplicationUser User { get; set; } = null!;
}