namespace ExpenseTracker.Core.Entities;

public class Expense : BaseEntity
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public int? ReceiptId { get; set; }
    public Receipt? Receipt { get; set; }
}
