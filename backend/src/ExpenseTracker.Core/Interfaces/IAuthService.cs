using ExpenseTracker.Core.DTOs;
using ExpenseTracker.Core.Entities;

namespace ExpenseTracker.Core.Interfaces;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterDto registerDto);
    Task<User?> ValidateUserAsync(LoginDto loginDto);
}
