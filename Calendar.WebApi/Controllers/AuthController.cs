using Calendar.Application.DTOs;
using Calendar.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Calendar.WebApi.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly CalendarDbContext _context;

    public AuthController(CalendarDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Login))
        {
            return BadRequest("Login is required.");
        }

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == request.Login);

        if (user == null)
        {
            return Unauthorized("Invalid login.");
        }

        return Ok(new
        {
            user.Id,
            user.Login,
            user.Name
        });
    }
}
