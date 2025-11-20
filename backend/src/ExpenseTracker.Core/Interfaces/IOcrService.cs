using ExpenseTracker.Core.DTOs;

namespace ExpenseTracker.Core.Interfaces;

public interface IOcrService
{
    Task<ReceiptDto> ScanReceiptAsync(Stream imageStream, string contentType, IEnumerable<CategoryDto> availableCategories);
}
