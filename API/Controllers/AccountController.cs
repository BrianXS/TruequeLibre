using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using API.Entities;
using API.Repositories.Implementations;
using API.Repositories.Interfaces;
using API.Resources.Incoming;
using API.Resources.Outgoing;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AccountController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ProfileInfoResponse>> GetMyProfile()
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
            if (userInfo != null)
            {
                var response = _mapper.Map<ProfileInfoResponse>(userInfo);
                return Ok();
            }

            return Unauthorized();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileInfoResponse>> GetProfile(int id)
        {
            var userInfo = await _userRepository.FindUserById(id);
            if (userInfo != null)
            {
                var response = _mapper.Map<ProfileInfoResponse>(userInfo);
                response.Products = _mapper.Map<List<ProfileInfoResponse.ProductInfo>>(userInfo.Products);
                return Ok(response);
            }
            
            return NotFound(new ProfileInfoResponse());
        }

        [HttpGet("Edit")]
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

        [HttpGet("Edit/Username")]
        public async Task<IActionResult> EditUserName(EditUserUserNameRequest request)
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
            if (userInfo != null)
            {
                await _userRepository.UpdateUserName(userInfo, request.UserName);
                return Ok(_mapper.Map<EditUserResponse>(userInfo));
            }
            
            return Unauthorized();
        }
        
        [HttpGet("Edit/Email")]
        public async Task<IActionResult> EditEmail(EditUserEmailRequest request)
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
            if (userInfo != null)
            {
                await _userRepository.UpdateEmail(userInfo, request.Email);
                return Ok(_mapper.Map<EditUserResponse>(userInfo));
            }
            
            return Unauthorized();
        }
        
        [HttpGet("Edit/Name")]
        public async Task<IActionResult> EditName(EditUserNameRequest request)
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
            if (userInfo != null)
            {
                await _userRepository.UpdateName(userInfo, request.Names, request.LastNames);
                return Ok(_mapper.Map<EditUserResponse>(userInfo));
            }
            
            return Unauthorized();
        }
        
        [HttpGet("Edit/PhoneNumber")]
        public async Task<IActionResult> EditPhoneNumber(EditUserPhoneNumberRequest request)
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
            if (userInfo != null)
            {
                await _userRepository.UpdateUserPhone(userInfo, request.PhoneNumber);
                return Ok(_mapper.Map<EditUserResponse>(userInfo));
            }
            
            return Unauthorized();
        }
    }
}