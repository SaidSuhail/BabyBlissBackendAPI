using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BabyBlissBackendAPI.Data;
using BabyBlissBackendAPI.Dto;
using BabyBlissBackendAPI.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BabyBlissBackendAPI.Services.AuthServices
{
    public class AuthServices:IAuthServices
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AuthServices(AppDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }
       

        public async Task<bool> Register(UserRegistrationDto userRegistrationDto)
        {
            try
            {
                var isExists = await _context.users.FirstOrDefaultAsync(a => a.UserEmail == userRegistrationDto.UserEmail);
                if (isExists != null)
                {
                    return false; // User already exists
                }

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegistrationDto.Password);

                var user = _mapper.Map<User>(userRegistrationDto);
                user.Password = hashedPassword;

                //// If the Role is provided, use it; otherwise, set default to 'User'
                //if (Enum.TryParse(userRegistrationDto.Role, true, out User.UserRole role))
                //{
                //    // Assign the string value of the role enum
                //    user.Role = role; // 'User' or 'Admin'
                //}
                //else
                //{
                    user.Role = User.UserRole.User; // Default to 'User' if invalid or not provided
                //}

                _context.users.Add(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
        }


        public async Task<UserResponseDto> Login(UserLoginDto userLoginDto)
        {
            try
            {
                var u = await _context.users.FirstOrDefaultAsync(a => a.UserEmail == userLoginDto.UserEmail);
                if (u == null)
                {
                    return new UserResponseDto { Error = "Not Found" };
                }
                var pass = validatePassword(userLoginDto.Password, u.Password);
                if (!pass)
                {
                    return new UserResponseDto { Error = "Invalid Password" };
                }
                if (u.IsBlocked == true)
                {
                    return new UserResponseDto { Error = "User Blocked" };
                }
                var Token = Generate_Token(u);
                return new UserResponseDto
                {
                    UserName = u.UserName,
                    Token = Token,
                    UserEmail = u.UserEmail,
                    Role = u.Role.ToString(),
                    Id = u.Id
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        private bool validatePassword(string password, string hashpassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashpassword);
        }

        private string Generate_Token(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentails = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claim = new[]
            {
                new Claim(ClaimTypes.NameIdentifier ,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName ),
                new Claim(ClaimTypes.Role,user.Role.ToString()),
                new Claim(ClaimTypes.Email,user.UserEmail)
            };

            var token = new JwtSecurityToken(
                claims: claim,
                signingCredentials: credentails,
                expires: DateTime.UtcNow.AddDays(1)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
