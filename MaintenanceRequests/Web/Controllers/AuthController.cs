using MaintenanceRequests.Application.DTOs.Auth;
using MaintenanceRequests.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceRequests.Web.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        if (!User.Identity?.IsAuthenticated ?? true)
        {
            return Unauthorized();
        }

        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Unauthorized();
        }

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new
        {
            user.Id,
            user.UserName,
            user.Email,
            roles
        });
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(x => x.Description));
        }

        await _userManager.AddToRoleAsync(user, "Client");

        return Ok(new
        {
            message = "Пользователь зарегистрирован."
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _signInManager.PasswordSignInAsync(
            dto.UserName,
            dto.Password,
            isPersistent: true,
            lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return Unauthorized(new
            {
                error = "Неверное имя пользователя или пароль."
            });
        }

        return Ok(new
        {
            message = "Вход выполнен."
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new
        {
            message = "Выход выполнен."
        });
    }
}