using System.Security.Claims;
using ExpenseTracker.Core.DTOs;
using ExpenseTracker.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpenses()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var expenses = await _expenseService.GetExpensesAsync(userId);
        return Ok(expenses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseDto>> GetExpense(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var expense = await _expenseService.GetExpenseByIdAsync(id, userId);
        if (expense == null) return NotFound();
        return Ok(expense);
    }

    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> CreateExpense(CreateExpenseDto createExpenseDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        try
        {
            var expense = await _expenseService.CreateExpenseAsync(createExpenseDto, userId);
            return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(int id, CreateExpenseDto createExpenseDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        try
        {
            await _expenseService.UpdateExpenseAsync(id, createExpenseDto, userId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        try
        {
            await _expenseService.DeleteExpenseAsync(id, userId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var categories = await _expenseService.GetCategoriesAsync(userId);
        return Ok(categories);
    }
}
