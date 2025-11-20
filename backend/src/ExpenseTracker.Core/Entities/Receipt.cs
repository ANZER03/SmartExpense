namespace ExpenseTracker.Core.Entities;

public class Receipt : BaseEntity
{
    public string ImageUrl { get; set; } = string.Empty;
    public string? MerchantName { get; set; }
    public decimal? TotalAmount { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string? RawText { get; set; } // OCR extracted text
    
    public int ExpenseId { get; set; }
    public Expense Expense { get; set; } = null!;
}
