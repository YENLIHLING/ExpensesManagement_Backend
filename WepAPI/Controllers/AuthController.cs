using BusinessLogicLayer;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer;
using WepAPI.Dto;

namespace WepAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly ExpensesManagementContext _context;
        private readonly TokenService _tokenService;
        private readonly PasswordHasher<PersonalModel> _passwordHasher = new();

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public AuthController(ExpensesManagementContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(PersonalDto dto)
        {
            if (_context.personal.Any(u => u.name == dto.Username))
                return BadRequest("Username already exists.");

            var personal = new PersonalModel { name = dto.Username };
            personal.passwordHash = _passwordHasher.HashPassword(personal, dto.Password);

            _context.personal.Add(personal);
            await _context.SaveChangesAsync();

            return Ok("User registered.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(PersonalDto dto)
        {
            var user = await _context.personal.FirstOrDefaultAsync(u => u.name == dto.Username);
            if (user == null) return Unauthorized("Invalid credentials.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.passwordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid credentials.");

            var token = _tokenService.CreateToken(user);
            return Ok(new { token });
        }
    }
}
