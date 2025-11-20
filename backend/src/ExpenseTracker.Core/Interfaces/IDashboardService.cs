using ExpenseTracker.Core.DTOs;

namespace ExpenseTracker.Core.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardDataAsync(int userId);
}
