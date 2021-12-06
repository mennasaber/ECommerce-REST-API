﻿using FirstCore.Data;
using FirstCore.Data.Dtos;
using FirstCore.IdentityAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FirstCore.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;
        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("registeruser")]
        public async Task<IActionResult> RegisterUser(RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(registerDto.Email);
                if (user != null)
                {
                    return BadRequest(new ResponseDto { Message = "User already exist" });
                }
                user = new ApplicationUser { UserName = registerDto.Username, Email = registerDto.Email };
                var isCreated = await _userManager.CreateAsync(user, registerDto.Password);
                if (isCreated.Succeeded)
                {
                    var role = await _roleManager.RoleExistsAsync(AppConstants.UserRole);
                    if (!role)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(AppConstants.UserRole));
                    }
                    await _userManager.AddToRoleAsync(user, AppConstants.UserRole);
                    return Ok();
                }
                var responseDto = new ResponseDto();
                foreach (var error in isCreated.Errors)
                {
                    responseDto.Message += error.Description + " ";
                }
                return BadRequest(responseDto);
            }
            return BadRequest(new ResponseDto { Message = "InValid" });
        }

        [HttpPost]
        [Route("registeradmin")]
        public async Task<IActionResult> RegisterAdmin(RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(registerDto.Email);
                if (user != null)
                {
                    return BadRequest();
                }
                user = new ApplicationUser { UserName = registerDto.Username, Email = registerDto.Email };
                var isCreated = await _userManager.CreateAsync(user, registerDto.Password);
                if (isCreated.Succeeded)
                {
                    var role = await _roleManager.RoleExistsAsync(AppConstants.AdminRole);
                    if (!role)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(AppConstants.AdminRole));
                    }
                    await _userManager.AddToRoleAsync(user, AppConstants.AdminRole);
                    return Ok();
                }
                var responseDto = new ResponseDto();
                foreach (var error in isCreated.Errors)
                {
                    responseDto.Message += error.Description + " ";
                }
                return BadRequest(responseDto);
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user != null)
            {
                var isValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (isValid)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var token = GenerateJwtToken(user, userRoles);
                    return Ok(new ResponseDto { Message = token });
                }
            }
            return BadRequest();
        }
        private string GenerateJwtToken(IdentityUser user, IList<string> userRoles)
        {
            // Now its ime to define the jwt token which will be responsible of creating our tokens
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // We get our secret from the appsettings
            var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]);

            var authClaims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                // the JTI is used for our refresh token which we will be convering in the next video
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            // we define our token descriptor
            // We need to utilise claims which are properties in our token which gives information about the token
            // which belong to the specific user who it belongs to
            // so it could contain their id, name, email the good part is that these information
            // are generated by our server and identity framework which is valid and trusted
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                // the life span of the token needs to be shorter and utilise refresh token to keep the user signedin
                // but since this is a demo app we can extend it to fit our current need
                Expires = DateTime.UtcNow.AddHours(6),
                // here we are adding the encryption alogorithim information which will be used to decrypt our token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}