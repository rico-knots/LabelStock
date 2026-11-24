# Labelstock

## Update database
1. Maak wijzigingen aan in `LabelStockDbContext`
2. run `dotnet ef migrations add {naam}` - naam is soort commit message
3. run `dotnet ef database update`
4. 