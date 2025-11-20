using System.Security.Claims;
using ExpenseTracker.Core.DTOs;
using ExpenseTracker.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<ActionResult<DashboardDto>> GetDashboardData()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var dashboardData = await _dashboardService.GetDashboardDataAsync(userId);
        return Ok(dashboardData);
    }
}
