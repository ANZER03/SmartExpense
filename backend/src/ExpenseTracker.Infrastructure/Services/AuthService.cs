using ExpenseTracker.Core.DTOs;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Core.Interfaces;

namespace ExpenseTracker.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
    {
        // Check if user exists
        var existingUsers = await _unitOfWork.Repository<User>().FindAsync(u => u.Email == registerDto.Email);
        if (existingUsers.Any())
        {
            throw new Exception("User with this email already exists.");
        }

        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
        };

        await _unitOfWork.Repository<User>().AddAsync(user);
        await _unitOfWork.CompleteAsync();

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        };
    }

    public async Task<User?> ValidateUserAsync(LoginDto loginDto)
    {
        var users = await _unitOfWork.Repository<User>().FindAsync(u => u.Email == loginDto.Email);
        var user = users.FirstOrDefault();

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            return null;
        }

        return user;
    }
}
