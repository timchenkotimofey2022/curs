using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentPerformanceApi.Auth;
using StudentPerformanceApi.DTOs;
using StudentPerformanceApi.Models;
using StudentPerformanceApi.Repositories.Interfaces;

namespace StudentPerformanceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly IUserRepository _userRepository;

    public AuthController(JwtService jwtService, IUserRepository userRepository)
    {
        _jwtService = jwtService;
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null || !PasswordHasher.Verify(dto.Password, user.PasswordHash))
        {
            return Unauthorized(new { Message = "Неверный email или пароль" });
        }

        var response = _jwtService.GenerateToken(user.Email, user.FullName, user.Role);
        return Ok(response);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (await _userRepository.ExistsByEmailAsync(dto.Email))
        {
            return BadRequest(new { Message = "Пользователь с таким email уже существует" });
        }

        if (string.IsNullOrWhiteSpace(dto.Role) ||
            (dto.Role != "Admin" && dto.Role != "Teacher" && dto.Role != "Student"))
        {
            return BadRequest(new { Message = "Роль должна быть Admin, Teacher или Student" });
        }

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = PasswordHasher.Hash(dto.Password),
            Role = dto.Role
        };

        await _userRepository.AddAsync(user);

        var response = _jwtService.GenerateToken(user.Email, user.FullName, user.Role);
        return Ok(response);
    }
}
