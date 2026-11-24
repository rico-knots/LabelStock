using LabelStock.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LabelStock.Core.Data;

public class LabelStockDbContext(DbContextOptions<LabelStockDbContext> options) : DbContext(options)
{
	public DbSet<StockEntry> StockEntries { get; set; }
    
	// For migrations, for regular config, done in DependencyInjection.cs
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseSqlite("Data Source=design_time_temp.db");
		}
	}
	
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
	    var now = DateTime.UtcNow;

	    foreach (var entry in ChangeTracker.Entries<StockEntry>())
	    {
		    if (entry.State == EntityState.Added)
		    {
			    entry.Entity.CreatedAt = now;
			    entry.Entity.UpdatedAt = now;
		    }
		    else if (entry.State == EntityState.Modified)
		    {
			    entry.Entity.UpdatedAt = now;
		    }
	    }

	    return base.SaveChangesAsync(cancellationToken);
    }
}