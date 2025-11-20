namespace ExpenseTracker.Core.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}
