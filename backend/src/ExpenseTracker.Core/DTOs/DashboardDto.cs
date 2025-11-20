namespace ExpenseTracker.Core.DTOs;

public class DashboardDto
{
    public decimal TotalSpent { get; set; }
    public decimal MonthlyBudget { get; set; } // Placeholder for now
    public List<ExpenseDto> RecentExpenses { get; set; } = new();
    public List<CategoryExpenseDto> ExpensesByCategory { get; set; } = new();
    public List<MonthlyExpenseDto> ExpensesByMonth { get; set; } = new();
    public List<DailyExpenseDto> ExpensesLast10Days { get; set; } = new();
}

public class CategoryExpenseDto
{
    public string CategoryName { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class MonthlyExpenseDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class DailyExpenseDto
{
    public string Date { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
