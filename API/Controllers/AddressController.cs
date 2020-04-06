using System.Threading.Tasks;
using API.Entities;
using API.Repositories.Interfaces;
using API.Resources.Incoming;
using API.Resources.Outgoing;
using API.Services.User;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("account/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AddressController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ICurrentUserInfo _currentUserInfo;
        private readonly IMapper _mapper;

        public AddressController(IUserRepository userRepository,
                                IAddressRepository addressRepository,
                                ICurrentUserInfo currentUserInfo,
                                IMapper mapper)
        {
            _userRepository = userRepository;
            _addressRepository = addressRepository;
            _currentUserInfo = currentUserInfo;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(AddAddressRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var address = _mapper.Map<Address>(request);
            address.UserId = await _currentUserInfo.GetCurrentUserId();
            
            _addressRepository.SaveAddress(address);
            return Ok();
        }
        
        [HttpGet("{addressId}")]
        public async Task<ActionResult<GetAddressResponse>> GetAddressInfo(int addressId)
        {
            var userId = await _currentUserInfo.GetCurrentUserId();
            var address = _addressRepository.GetAddressWithCity(addressId);
            
            if (address == null || !userId.Equals(address.UserId))
                return NotFound();

            var response = _mapper.Map<GetAddressResponse>(address);
            return Ok(response);
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateAddress(UpdateAddressRequest request)
        {
            var userId = await _currentUserInfo.GetCurrentUserId();
            var address = _addressRepository.GetAddress(request.Id);

            if (!ModelState.IsValid)
                return BadRequest();
            
            if(address == null || !address.UserId.Equals(userId))
                return NotFound();

            _mapper.Map(request, address);
            _addressRepository.EditAddress(address);
            
            return Ok();
        }
        
        [HttpDelete("{addressId}")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            var userId = await _currentUserInfo.GetCurrentUserId();

            if (_addressRepository.GetAddress(addressId).UserId != userId)
                return Unauthorized();

            _addressRepository.DeleteAddress(addressId);
            return Ok();
        }
    }
}