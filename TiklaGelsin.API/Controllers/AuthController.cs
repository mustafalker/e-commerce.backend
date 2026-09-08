using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TiklaGelsin.Application.DTOs;
using TiklaGelsin.Application.Interfaces;
using TiklaGelsin.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace TiklaGelsin.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthController(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Kullanıcı adı ve şifre zorunludur.");

            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
                return BadRequest("Bu kullanıcı adı zaten alınmış.");

            var passwordHash = _passwordHasher.HashPassword(request.Password);
            var newUser = new User(Guid.NewGuid(), request.Username, passwordHash);

            await _userRepository.AddAsync(newUser);

            return Ok(new { Message = "Kullanıcı başarıyla oluşturuldu." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Kullanıcı adı ve şifre zorunludur.");

            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized("Geçersiz kullanıcı adı veya şifre.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("SuperSecretKeyForJwtAuthentication12345!!");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

        [Authorize]
        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            if (request.NewPassword != request.NewPasswordConfirm)
                return BadRequest("Yeni şifreler eşleşmiyor.");

            var currentUsername = User.Identity?.Name;
            if (currentUsername != request.Username)
                return BadRequest("Sadece kendi şifrenizi güncelleyebilirsiniz.");

            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            if (!_passwordHasher.VerifyPassword(request.OldPassword, user.PasswordHash))
                return BadRequest("Eski şifre hatalı.");

            var newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);
            user.UpdatePassword(newPasswordHash);

            await _userRepository.UpdateAsync(user);

            return Ok(new { Message = "Şifre başarıyla güncellendi." });
        }
    }
}
