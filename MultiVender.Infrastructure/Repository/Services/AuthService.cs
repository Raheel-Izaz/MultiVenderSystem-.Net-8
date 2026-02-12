using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MultiVender.Application.DTOs;
using MultiVender.Application.Interfaces;
using MultiVender.Application.IServices;
using MultiVender.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MultiVender.Infrastructure.Repository.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<User?> RegisterAsync(RegisterDto dto)
        {
            var existingUsers = await _unitOfWork.Users.GetAllAsync();
            if (existingUsers.Any(u => u.Email == dto.Email))
                return null;

            var userRole = (await _unitOfWork.Roles
                .GetAllAsync(r => r.RoleName == "User")).FirstOrDefault();

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = new PasswordHasher<User>()
                    .HashPassword(null!, dto.Password),
                RoleId = userRole!.Id
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveAsync();

            return user;
        }

        public async Task<TokenResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = (await _unitOfWork.Users
                .GetAllAsync(u => u.Email == dto.Email))
                .FirstOrDefault();

            if (user == null)
                return null;

            if (new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.PasswordHash, dto.Password)
                == PasswordVerificationResult.Failed)
                return null;

            return await CreateTokenResponse(user);
        }

        public async Task<TokenResponseDto?> RefreshTokenAsync(int userId, string refreshToken)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);

            if (user == null ||
                user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return null;

            return await CreateTokenResponse(user);
        }

        private async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        private string CreateToken(User user)
        {
            var role = _unitOfWork.Roles
                .GetAllAsync(r => r.Id == user.RoleId)
                .Result.First();

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, role.RoleName)
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            user.RefreshToken = Convert.ToBase64String(randomBytes);
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.SaveAsync();

            return user.RefreshToken;
        }
    }
}
