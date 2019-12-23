using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Entities;
using API.Repositories.Implementations;
using API.Repositories.Interfaces;
using API.Resources.Incoming;
using API.Resources.Outgoing;
using API.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public AccountController(IUserRepository userRepository,
                                 IAddressRepository addressRepository,
                                 IMapper mapper)
        {
            _userRepository = userRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GetProfileResponse>> GetMyProfile()
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
            if (userInfo != null)
            {
                var response = _mapper.Map<GetProfileResponse>(userInfo);
                response.Products = _mapper.Map<List<GetProfileResponse.ProductInfo>>(userInfo.Products);
                response.Addresses = _mapper
                    .Map<List<GetProfileResponse.AddressInfo>>(_addressRepository.GetUserAddresses(userInfo.Id));
                return Ok(response);
            }

            return Unauthorized();
        }

        [HttpGet("{id}"), AllowAnonymous]
        public async Task<ActionResult<GetProfileResponse>> GetProfile(int id)
        {
            var userInfo = await _userRepository.FindUserById(id);
            
            if (userInfo != null)
            {
                var response = _mapper.Map<GetProfileResponse>(userInfo);
                response.Products = _mapper.Map<List<GetProfileResponse.ProductInfo>>(userInfo.Products);
                return Ok(response);
            }
            
            return NotFound(new GetProfileResponse());
        }

        [HttpGet("Update")]
        public async Task<ActionResult<EditUserResponse>> GetMyProfileInfo()
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
            if (userInfo != null)
            {
                return Ok(_mapper.Map<EditUserResponse>(userInfo));
            }
            
            return Unauthorized();
        }

        [HttpPost("Update/Username")]
        public async Task<ActionResult<UpdateUserUserNameResponse>> UpdateUserName(UpdateUserUserNameRequest request)
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
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
            if (!ModelState.IsValid) return BadRequest();
            
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
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
            if (!ModelState.IsValid) return BadRequest();
            
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
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
            if (!ModelState.IsValid) return BadRequest();
            
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
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
            if (!ModelState.IsValid) return BadRequest();
            
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
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