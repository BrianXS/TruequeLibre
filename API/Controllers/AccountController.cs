using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Repositories.Interfaces;
using API.Resources.Incoming;
using API.Resources.Outgoing;
using API.Services.User;
using API.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserInfo _currentUserInfo;
        private readonly IMapper _mapper;

        public AccountController(IUserRepository userRepository,
                                 ICurrentUserInfo currentUserInfo,
                                 IMapper mapper)
        {
            _userRepository = userRepository;
            _currentUserInfo = currentUserInfo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<ProfileResponse> GetMyProfile()
        {
            
            var userInfo = _currentUserInfo.GetCurrentUserResource();
            if (userInfo != null) 
                return Ok(userInfo);
            
            return NotFound();
        }

        [HttpGet("{id}"), AllowAnonymous]
        public ActionResult<ProfileResponse> GetProfile(int id)
        {
            var userInfo = _userRepository.FindProfileResponseById(id);
            
            if (userInfo != null)
            {
                var response = _mapper.Map<ProfileResponse>(userInfo);
                return Ok(response);
            }
            
            return NotFound();
        }

        [HttpGet("Update")]
        public ActionResult<UpdateUserResponse> GetMyProfileInfo()
        {
            var userInfo = _currentUserInfo.GetCurrentUserUpdateResource();

            if (userInfo != null) 
                return Ok(userInfo);

            return NotFound();
        }

        [HttpPost("Update/Username")]
        public async Task<ActionResult<UpdateUserUserNameResponse>> UpdateUserName(UpdateUserUserNameRequest request)
        {
            var userInfo = await _currentUserInfo.GetCurrentUser();
            if (userInfo != null)
            {
                await _userRepository.UpdateUserName(userInfo, request.UserName);
                var roles = await _userRepository.GetUserRoles(userInfo);
                
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
                
                var response = new UpdateUserUserNameResponse
                {
                  Token  = TokenUtils.TokenGenerator(claims.ToArray()),
                  RefreshToken = userInfo.RefreshToken
                };
                
                return Ok(response);
            }
            
            return Unauthorized();
        }
        
        [HttpPost("Update/Email")]
        public async Task<IActionResult> EditEmail(UpdateUserEmailRequest request)
        {
            if (!ModelState.IsValid) 
                return BadRequest();
            
            var userInfo = await _currentUserInfo.GetCurrentUser();
            if (userInfo != null)
            {
                await _userRepository.UpdateEmail(userInfo, request.Email);
                return Ok();
            }
            
            return Unauthorized();
        }
        
        [HttpPost("Update/Name")]
        public async Task<IActionResult> UpdateName(UpdateUserNameRequest request)
        {
            var userInfo = await _currentUserInfo.GetCurrentUser();
            
            if (!ModelState.IsValid) 
                return BadRequest();
            
            if (userInfo != null)
            {
                await _userRepository.UpdateName(userInfo, request.Names, request.LastNames);
                return Ok();
            }
            
            return Unauthorized();
        }
        
        [HttpPost("Update/PhoneNumber")]
        public async Task<IActionResult> UpdatePhoneNumber(UpdateUserPhoneNumberRequest request)
        {
            var userInfo = await _currentUserInfo.GetCurrentUser();
            
            if (!ModelState.IsValid) 
                return BadRequest();
            
            if (userInfo != null)
            {
                await _userRepository.UpdateUserPhone(userInfo, request.PhoneNumber);
                return Ok();
            }
            
            return Unauthorized();
        }

        [HttpPost("Update/Password")]
        public async Task<IActionResult> UpdatePassword(UpdateUserPasswordRequest request)
        {
            var userInfo = await _currentUserInfo.GetCurrentUser();
            
            if (!ModelState.IsValid) 
                return BadRequest();
            
            if (userInfo != null)
            {
                var result = await _userRepository.UpdatePassword(userInfo, request.OldPassword, request.NewPassword);
                
                if(result) 
                    return Ok();
                
                return UnprocessableEntity();
            }
            
            return Unauthorized();
        }
    }
}