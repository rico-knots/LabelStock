using LabelStock.Core.Data;

namespace LabelStock.Core.Services;

using LabelStock.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

public class StockService(IDbContextFactory<LabelStockDbContext> dbFactory) : IStockService
{
	public async Task SetQuantityAsync(int labelId, int quantity)
	{
		await using var db = await dbFactory.CreateDbContextAsync();

		var entry = await db.StockEntries.SingleAsync(s => s.Id == labelId);
		entry.Stock = Math.Max(0, quantity);
		entry.UpdatedAt = DateTime.UtcNow;

		await db.SaveChangesAsync();
	}
}