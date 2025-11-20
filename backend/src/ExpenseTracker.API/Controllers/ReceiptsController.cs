using System.Security.Claims;
using ExpenseTracker.Core.DTOs;
using ExpenseTracker.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReceiptsController : ControllerBase
{
    private readonly IOcrService _ocrService;
    private readonly IExpenseService _expenseService;

    public ReceiptsController(IOcrService ocrService, IExpenseService expenseService)
    {
        _ocrService = ocrService;
        _expenseService = expenseService;
    }

    [HttpPost("scan")]
    public async Task<ActionResult<ReceiptDto>> ScanReceipt(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        // Get user ID from claims
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

        // Fetch user's categories to pass to LLM for category prediction
        var categories = await _expenseService.GetCategoriesAsync(userId);

        using var stream = file.OpenReadStream();
        var result = await _ocrService.ScanReceiptAsync(stream, file.ContentType, categories);

        return Ok(result);
    }
}
