using FirstCore.Data;
using FirstCore.Data.Dtos;
using FirstCore.IdentityAuth;
using FirstCore.IRepos;
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
        private readonly IAuthRepo _authRepo;
        public AuthenticateController(IAuthRepo authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost]
        [Route("registeruser")]
        public async Task<IActionResult> RegisterUser(RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _authRepo.RegisterUser(registerDto);
                return StatusCode(response.Status,response.Message);
            }
            return BadRequest("InValid");
        }

        [HttpPost]
        [Route("registeradmin")]
        public async Task<IActionResult> RegisterAdmin(RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _authRepo.RegisterAdmin(registerDto);
                return StatusCode(response.Status, response.Message);
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var response = await _authRepo.Login(loginDto);
            return StatusCode(response.Status, response.Message);
        }
    }
}
