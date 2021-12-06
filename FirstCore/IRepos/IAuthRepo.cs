using FirstCore.Data.Dtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCore.IRepos
{
    public interface IAuthRepo
    {
        Task<ResponseDto> RegisterAdmin(RegisterDto registerDto);
        Task<ResponseDto> RegisterUser(RegisterDto registerDto);
        Task<ResponseDto> Register(RegisterDto registerDto, string role);
        Task<ResponseDto> Login(LoginDto login);
        string GenerateJwtToken(IdentityUser user, IList<string> userRoles);
    }
}
