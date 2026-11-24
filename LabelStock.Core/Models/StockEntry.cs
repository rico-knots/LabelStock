namespace LabelStock.Core.Models;

public class StockEntry
{
	public int Id { get; set; }
	public string Barcode { get; set; }
	public string LabelName { get; set; }
    public int Stock { get; set; }
    public bool Locked { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public StockEntry(string barcode, string labelName, int stock,  bool locked)
    {
	    Barcode = barcode;
	    LabelName = labelName;
        Stock = stock;
        Locked = locked;
    }
}