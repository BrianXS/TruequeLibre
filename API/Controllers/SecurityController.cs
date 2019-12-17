using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Entities;
using API.Resources.Incoming;
using API.Resources.Outgoing;
using API.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public SecurityController(UserManager<User> userManager, 
                                  SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _signInManager.PasswordSignInAsync(loginRequest.UserName,
                loginRequest.Password, false, false);

            if (result.Succeeded)
            {
                var userInfo = await _userManager.FindByNameAsync(loginRequest.UserName);
                var userRoles = await _userManager.GetRolesAsync(userInfo);
                
                userInfo.RefreshToken = TokenUtils.RefreshToken();
                await _userManager.UpdateAsync(userInfo);

                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, userRoles[0]),
                };
                
                var response = new LoginResponse
                {
                    Token = TokenUtils.TokenGenerator(claims),
                    RefreshToken = userInfo.RefreshToken
                };

                return Ok(response);
            }

            return Unauthorized(new LoginResponse());
        }
        
        [HttpPost("refresh")]
        public async Task<ActionResult<RefreshTokenResponse>> RefreshToken([FromBody] RefreshTokenRequest refreshToken)
        {
            return null;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest registerRequest)
        {
            return null;
        }

    }
}