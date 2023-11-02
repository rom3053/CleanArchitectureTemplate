using CleanArchitectureTemplate.Application.Common.Constants;
using CleanArchitectureTemplate.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureTemplate.Infrastructure.Persistence;

/// <summary>
/// Db context initializer
/// </summary>
public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new ApplicationRole { Name = Roles.Admin };

        await AddRoleIfNotExistingAsync(administratorRole);

        // Default users
        await AddAdminAsync("dummy@mail.com", "Password123$");
        await _context.SaveChangesAsync();
    }

    private async Task AddRoleIfNotExistingAsync(ApplicationRole role)
    {
        if (_roleManager.Roles.All(r => r.Name != role.Name))
        {
            await _roleManager.CreateAsync(role);
        }
    }

    private async Task AddAdminAsync(string userEmail, string userPassword, string roleName = Roles.Admin)
    {
        if (_userManager.Users.All(u => u.Email == userEmail))
        {
            return;
        }

        var newUser = new ApplicationUser()
        {
            PhoneNumber = "3800000",
            Email = userEmail,
            UserName = userEmail,
        };

        var password = userPassword;
        newUser.PasswordHash = _userManager.PasswordHasher.HashPassword(newUser, password);

        await _userManager.CreateAsync(newUser);

        await _userManager.AddToRolesAsync(newUser, new[] { roleName });
    }
}
