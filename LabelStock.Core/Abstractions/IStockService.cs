namespace LabelStock.Core.Abstractions;

public interface IStockService
{
    /// <summary>Sets the stock of a label to an exact number (never below zero).</summary>
    Task SetQuantityAsync(int labelId, int quantity);
}
