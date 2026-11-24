using LabelStock.Core.Abstractions;
using LabelStock.Core.Data;
using LabelStock.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LabelStock.Core.Services;

public class LabelService(IDbContextFactory<LabelStockDbContext> dbFactory) : ILabelService
{
	public async Task<IReadOnlyList<LabelSummary>> SearchAsync(string query)
	{
		
		if ( string.IsNullOrWhiteSpace(query) )
		{
			return [];
		}
		
		await using var db = await dbFactory.CreateDbContextAsync();

		var exact = await db.StockEntries
			.AsNoTracking()
			.Where(e => e.Barcode == query)
			.Select(e => EntryToSummary(e))
			.FirstOrDefaultAsync();

		if ( exact is not null )
		{
			return [exact];
		}
		
		return await db.StockEntries
			.AsNoTracking()
			.Where(e => e.LabelName.Contains(query) || e.Barcode.Contains(query))
			.Select(e => EntryToSummary(e))
			.ToListAsync();
	}

	public async Task<IReadOnlyList<LabelSummary>> GetRecentAsync(int count)
	{
		await using var db = await dbFactory.CreateDbContextAsync();

		return await db.StockEntries
			.AsNoTracking()
			.OrderByDescending(e => e.UpdatedAt)
			.Take(count)
			.Select(e => EntryToSummary(e))
			.ToListAsync();
	}
	
	public async Task<IReadOnlyList<LabelSummary>> GetAllAsync(string? filter)
	{
		await using var db = await dbFactory.CreateDbContextAsync();

		var query = db.StockEntries.AsNoTracking();

		if (!string.IsNullOrWhiteSpace(filter))
		{
			query = query.Where(e =>
				e.LabelName.Contains(filter) ||
				e.Barcode.Contains(filter));
		}

		return await query
			.OrderBy(e => e.LabelName)
			.Select(e => EntryToSummary(e))
			.ToListAsync();
	}

	public async Task<LabelSummary> CreateAsync(string barcode, string name, int quantity)
	{
		await using var db = await dbFactory.CreateDbContextAsync();

		var exists = await db.StockEntries.AnyAsync(l => l.Barcode == barcode);
		if (exists)
		{
			throw new InvalidOperationException("This barcode is already in the system.");
		}

		var stockEntry = new StockEntry(barcode, name, quantity, false);;

		db.StockEntries.Add(stockEntry);
		await db.SaveChangesAsync();

		return EntryToSummary(stockEntry);
	}

	public async Task DeleteAsync(int entryId)
	{
		await using var db = await dbFactory.CreateDbContextAsync();

		await db.StockEntries
			.Where(e => e.Id == entryId)
			.ExecuteDeleteAsync();
	}


	private static LabelSummary EntryToSummary(StockEntry entry)
	{
		return new LabelSummary(entry.Id, entry.Barcode, entry.LabelName, entry.Stock);
	}
}