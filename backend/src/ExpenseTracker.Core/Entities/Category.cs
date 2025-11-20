namespace ExpenseTracker.Core.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#000000"; // Hex code
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
