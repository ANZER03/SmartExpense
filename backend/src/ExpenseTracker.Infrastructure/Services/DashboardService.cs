using ExpenseTracker.Core.DTOs;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Core.Interfaces;

namespace ExpenseTracker.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DashboardDto> GetDashboardDataAsync(int userId)
    {
        var expenses = await _unitOfWork.Repository<Expense>().FindAsync(e => e.UserId == userId);
        var categories = await _unitOfWork.Repository<Category>().FindAsync(c => c.UserId == userId); // Fetch categories for names/colors

        var expenseList = expenses.ToList();
        var categoryList = categories.ToList();

        var totalSpent = expenseList.Sum(e => e.Amount);

        var expensesByCategory = expenseList
            .GroupBy(e => e.CategoryId)
            .Select(g => new CategoryExpenseDto
            {
                CategoryName = categoryList.FirstOrDefault(c => c.Id == g.Key)?.Name ?? "Unknown",
                Color = categoryList.FirstOrDefault(c => c.Id == g.Key)?.Color ?? "#000000",
                Amount = g.Sum(e => e.Amount)
            })
            .ToList();

        var expensesByMonth = expenseList
            .GroupBy(e => new { e.Date.Year, e.Date.Month })
            .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
            .Select(g => new MonthlyExpenseDto
            {
                Month = $"{g.Key.Month}/{g.Key.Year}",
                Amount = g.Sum(e => e.Amount)
            })
            .ToList();

        return new DashboardDto
        {
            TotalSpent = totalSpent,
            MonthlyBudget = 0, // Implement budget logic later
            ExpensesByCategory = expensesByCategory,
            ExpensesByMonth = expensesByMonth
        };
    }
}
