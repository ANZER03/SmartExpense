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

    public ReceiptsController(IOcrService ocrService)
    {
        _ocrService = ocrService;
    }

    [HttpPost("scan")]
    public async Task<ActionResult<ReceiptDto>> ScanReceipt(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        using var stream = file.OpenReadStream();
        var result = await _ocrService.ScanReceiptAsync(stream, file.ContentType);

        return Ok(result);
    }
}
