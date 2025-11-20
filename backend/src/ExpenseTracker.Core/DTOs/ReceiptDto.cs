namespace ExpenseTracker.Core.DTOs;

public class ReceiptDto
{
    public string MerchantName { get; set; } = string.Empty;
    public decimal? TotalAmount { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string? SuggestedCategoryName { get; set; }
    public string RawText { get; set; } = string.Empty;
}
