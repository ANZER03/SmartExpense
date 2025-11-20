using AutoMapper;
using ExpenseTracker.Core.DTOs;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Core.Interfaces;

namespace ExpenseTracker.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DashboardService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DashboardDto> GetDashboardDataAsync(int userId)
    {
        var expenses = await _unitOfWork.Repository<Expense>().FindAsync(e => e.UserId == userId);
        var categories = await _unitOfWork.Repository<Category>().FindAsync(c => c.UserId == userId); // Fetch categories for names/colors

        var expenseList = expenses.ToList();
        var categoryList = categories.ToList();

        var totalSpent = expenseList.Sum(e => e.Amount);

        var recentExpenses = expenseList
            .OrderByDescending(e => e.Date)
            .Take(5)
            .ToList();

        var recentExpenseDtos = _mapper.Map<List<ExpenseDto>>(recentExpenses);

        // Map categories to recent expense DTOs manually
        foreach (var expenseDto in recentExpenseDtos)
        {
            var category = categoryList.FirstOrDefault(c => c.Id == expenseDto.CategoryId);
            if (category != null)
            {
                expenseDto.CategoryName = category.Name;
                expenseDto.CategoryColor = category.Color;
            }
            else
            {
                expenseDto.CategoryName = "Unknown";
                expenseDto.CategoryColor = "#808080"; // Gray
            }
        }

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

        // Calculate last 10 days expenses
        var last10Days = Enumerable.Range(0, 10)
            .Select(i => DateTime.Today.AddDays(-i))
            .OrderBy(d => d)
            .ToList();

        var expensesLast10Days = last10Days
            .Select(date => new DailyExpenseDto
            {
                Date = date.ToString("MM/dd"),
                Amount = expenseList
                    .Where(e => e.Date.Date == date.Date)
                    .Sum(e => e.Amount)
            })
            .ToList();

        return new DashboardDto
        {
            TotalSpent = totalSpent,
            MonthlyBudget = 0, // Implement budget logic later
            RecentExpenses = recentExpenseDtos,
            ExpensesByCategory = expensesByCategory,
            ExpensesByMonth = expensesByMonth,
            ExpensesLast10Days = expensesLast10Days
        };
    }
}
