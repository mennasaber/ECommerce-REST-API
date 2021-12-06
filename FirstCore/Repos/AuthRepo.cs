using FirstCore.Data;
using FirstCore.Data.Dtos;
using FirstCore.IdentityAuth;
using FirstCore.IRepos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FirstCore.Repos
{
    public class AuthRepo : IAuthRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ICustomersRepo _customersRepo;
        private readonly IOwnerRepo _ownerRepo;


        public AuthRepo(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ICustomersRepo customersRepo, IOwnerRepo ownerRepo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _customersRepo = customersRepo;
            _ownerRepo = ownerRepo;
        }

        public async Task<ResponseDto> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user != null)
            {
                var isValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (isValid)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var token = GenerateJwtToken(user, userRoles);
                    return new ResponseDto
                    {
                        Status = AppConstants.OkStatus,
                        Result = true,
                        Token = token
                    };
                }
            }
            return new ResponseDto
            {
                Status = AppConstants.NotFoundStatus,
                Result = false,
                Errors = new List<string>() { AppConstants.InvalidUsernameAndPassword }
            };
        }

        public async Task<ResponseDto> RegisterAdmin(RegisterDto registerDto)
        {
            return await Register(registerDto, AppConstants.AdminRole);
        }

        public async Task<ResponseDto> RegisterUser(RegisterDto registerDto)
        {
            return await Register(registerDto, AppConstants.UserRole);
        }

        public async Task<ResponseDto> Register(RegisterDto registerDto, string role)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            if (user != null)
            {
                return new ResponseDto
                {
                    Status = AppConstants.BadRequestStatus,
                    Result = false,
                    Errors = new List<string>() { AppConstants.UserAlreadyExist }
                };
            }
            user = new ApplicationUser { UserName = registerDto.Username, Email = registerDto.Email };
            var isCreated = await _userManager.CreateAsync(user, registerDto.Password);
            if (isCreated.Succeeded)
            {
                var roleExist = await _roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
                await _userManager.AddToRoleAsync(user, role);
                if (role.Equals(AppConstants.UserRole))
                    await _customersRepo.AddAsync(new CustomerDto { userId = user.Id });
                else
                    await _ownerRepo.AddAsync(new OwnerDto { UserId = user.Id });
                return new ResponseDto
                {
                    Status = AppConstants.CreatedStatus,
                    Result = false,
                    Errors = new List<string>() { AppConstants.UserCreated }
                };
            }
            var responseDto = new ResponseDto()
            {
                Status = AppConstants.BadRequestStatus,
                Result = false,
                Errors = isCreated.Errors.Select(e => e.Description).ToList()
            };

            return responseDto;
        }

        public string GenerateJwtToken(IdentityUser user, IList<string> userRoles)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]);

            var authClaims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }

    }
}
