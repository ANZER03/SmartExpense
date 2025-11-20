using ExpenseTracker.Core.DTOs;

namespace ExpenseTracker.Core.Interfaces;

public interface IExpenseService
{
    Task<IEnumerable<ExpenseDto>> GetExpensesAsync(int userId);
    Task<ExpenseDto?> GetExpenseByIdAsync(int id, int userId);
    Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto createExpenseDto, int userId);
    Task UpdateExpenseAsync(int id, CreateExpenseDto createExpenseDto, int userId);
    Task DeleteExpenseAsync(int id, int userId);
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync(int userId);
}
