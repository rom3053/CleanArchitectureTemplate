using CleanArchitectureTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}
