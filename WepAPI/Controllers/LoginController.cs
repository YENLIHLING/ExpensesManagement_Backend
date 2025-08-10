using BusinessLogicLayer;
using BusinessLogicLayer.Dto;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer;
using System.Net;
using WepAPI.Common;
using WepAPI.Dto;

namespace WepAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly ExpensesManagementContext _context;
        private readonly TokenService _tokenService;
        private readonly PasswordHasher<PersonalModel> _passwordHasher = new();
        private readonly ILogger<LoginController> _logger;

        public LoginController(ExpensesManagementContext context, TokenService tokenService, ILogger<LoginController> logger)
        {
            _context = context;
            _tokenService = tokenService;
            _logger = logger; 
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(PersonalDto dtoPersonal)
        {
            if (_context.personal.Any(u => u.name == dtoPersonal.Username))
            {
                return ErrorHandling.Error((int)HttpStatusCode.BadRequest, "Invalid credentials.");
            }

            var personal = new PersonalModel { name = dtoPersonal.Username };
            personal.passwordHash = _passwordHasher.HashPassword(personal, dtoPersonal.Password);

            _context.personal.Add(personal);
            await _context.SaveChangesAsync();

            //"User registered."
            return new JsonResult(new ResultModel
            {
                status = (int)HttpStatusCode.OK
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(PersonalDto personalDto)
        {
            try
            {
                var user = await _context.personal.FirstOrDefaultAsync(u => u.name == personalDto.Username);
                if (user == null || VerifyUserHashPassword(user, personalDto) == PasswordVerificationResult.Failed)
                {
                    return ErrorHandling.Error((int)HttpStatusCode.Unauthorized, "Invalid credentials.");
                }

                var userInfo = new UserTokenInfo
                {
                    Id = user.id,
                    Name = user.name
                };

                var token = _tokenService.CreateToken(userInfo);
                return Ok(new { token });
            }

            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error during login.");
                return StatusCode(500, "An error occurred during login.");
            }
        }

        private PasswordVerificationResult VerifyUserHashPassword(PersonalModel user, PersonalDto dto)
        {
            return _passwordHasher.VerifyHashedPassword(user, user.passwordHash, dto.Password);
        }

    }
}
