using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Constants;
using API.Entities;
using API.Resources.Incoming;
using API.Resources.Outgoing;
using API.Utils;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public SecurityController(UserManager<User> userManager, 
                                  SignInManager<User> signInManager,
                                  IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
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
            var principal = TokenUtils.GetClaims(refreshToken.OldToken);
            var userName = principal.Identity.Name;
            var userInfo = await _userManager.FindByNameAsync(userName);
            
            if(string.IsNullOrWhiteSpace(userName) || userInfo == null)
                return BadRequest(new RefreshTokenResponse());

            userInfo.RefreshToken = TokenUtils.RefreshToken();
            await _userManager.UpdateAsync(userInfo);
            
            var userRoles = await _userManager.GetRolesAsync(userInfo);
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userRoles[0]),
            };
            
            var response = new RefreshTokenResponse
            {
                RefreshToken = userInfo.RefreshToken,
                Token = TokenUtils.TokenGenerator(claims)
            };
            
            return Ok(response);
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest registerRequest)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(registerRequest);

                await _userManager.CreateAsync(user, registerRequest.Password);
                return Ok(new RegisterResponse { Message = "User Created Successfully" });
            }
            
            return BadRequest(new RegisterResponse());
        }
    }
}