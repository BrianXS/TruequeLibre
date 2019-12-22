using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Entities;
using API.Repositories.Interfaces;
using API.Resources.Incoming;
using API.Resources.Outgoing;
using API.Services.Email;
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
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        public SecurityController(IUserRepository userRepository,
                                  SignInManager<User> signInManager,
                                  IEmailSender emailSender,
                                  IMapper mapper)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _mapper = mapper;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _signInManager.PasswordSignInAsync(loginRequest.UserName,
                loginRequest.Password, false, false);

            if (result.Succeeded)
            {
                var userInfo = await _userRepository.FindUserByName(loginRequest.UserName);
                var userRoles = await _userRepository.GetUserRoles(userInfo);

                await _userRepository.UpdateRefreshToken(userInfo, TokenUtils.RefreshToken());

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                var response = new LoginResponse
                {
                    Token = TokenUtils.TokenGenerator(claims.ToArray()),
                    RefreshToken = userInfo.RefreshToken
                };

                return Ok(response);
            }

            return Unauthorized(new LoginResponse());
        }
        
        [HttpPost("Refresh")]
        public async Task<ActionResult<RefreshTokenResponse>> RefreshToken([FromBody] RefreshTokenRequest refreshToken)
        {
            var principal = TokenUtils.GetClaims(refreshToken.OldToken);
            var userName = principal.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
            
            if(string.IsNullOrWhiteSpace(userName) || userInfo == null)
                return BadRequest(new RefreshTokenResponse());

            await _userRepository.UpdateRefreshToken(userInfo, TokenUtils.RefreshToken());
            
            var userRoles = await _userRepository.GetUserRoles(userInfo);
            var claims = new[]
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
        
        [HttpPost("Register")]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest registerRequest)
        {
            if(ModelState.IsValid)
            {
                var user = _mapper.Map<User>(registerRequest);

                await _userRepository.CreateUser(user, registerRequest.Password);
                return Ok(new RegisterResponse { Message = "User Created Successfully" });
            }
            
            return BadRequest(new RegisterResponse());
        }


        [HttpPost("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordRequest recoverPasswordRequest)
        {
            var user = await _userRepository.FindUserByName(recoverPasswordRequest.UserName);
            if (user != null)
            {
                var token = await _userRepository.GeneratePasswordResetTokenAsync(user);

                await _emailSender.SendEmailAsync(Constants.Email.OfficialEmailAddress, 
                    user.Email, "Reset Password", token, token);
                
                return Ok();
            }
            
            return NotFound(new { Message = "User Not Found" });
        }
        
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userRepository.FindUserByName(changePasswordRequest.UserName);
            
            if (user == null)
                return BadRequest();
        
            var changePasswordResult = await _userRepository.ResetPasswordAsync(user, 
                changePasswordRequest.Token, 
                changePasswordRequest.Password);

            if (changePasswordResult.Succeeded)
                return Ok();

            return Unauthorized();
        }
    }
}