using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.EntityFramework
{
  public class ConnDbContext : DbContext
  {
    public ConnDbContext(DbContextOptions<ConnDbContext> options) : base(options)
    {
    }
    public DbSet<Usuario> Usuarios { get; set; }
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
      IDbContextTransaction dbContextTransaction = null;

      if (Database.IsSqlServer() && Database.CurrentTransaction == null)
        dbContextTransaction = Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

      await SaveChangesAsync(cancellationToken);
      dbContextTransaction?.Commit();

      return true;
    }
  }
  
}
