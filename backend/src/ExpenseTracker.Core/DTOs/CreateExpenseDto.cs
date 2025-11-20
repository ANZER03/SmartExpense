using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Core.DTOs;

public class CreateExpenseDto
{
    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public int CategoryId { get; set; }
}
