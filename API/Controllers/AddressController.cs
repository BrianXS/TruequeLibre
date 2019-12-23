using System.Threading.Tasks;
using API.Entities;
using API.Repositories.Interfaces;
using API.Resources.Incoming;
using API.Resources.Outgoing;
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
        private readonly IMapper _mapper;

        public AddressController(IUserRepository userRepository,
                                IAddressRepository addressRepository,
                                IMapper mapper)
        {
            _userRepository = userRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddAddress(AddAddressRequest request)
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);

            if (userInfo == null)
                return Unauthorized();
            
            if (!ModelState.IsValid)
                return BadRequest();

            var address = _mapper.Map<Address>(request);
            address.UserId = userInfo.Id;
            
            _addressRepository.SaveAddress(address);
            return Ok();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAddressResponse>> GetAddressInfo(int id)
        {
            var address = _addressRepository.GetAddressWithCity(id);
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
            
            if (address == null || !userInfo.Id.Equals(address.UserId))
                return NotFound();

            var response = _mapper.Map<GetAddressResponse>(address);
            return Ok(response);
        }
        
        [HttpPost("Update")]
        public IActionResult UpdateAddress(UpdateAddressRequest request)
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = _userRepository.FindUserByName(userName);
            var address = _addressRepository.GetAddress(request.Id);

            if (!ModelState.IsValid)
                return BadRequest();
            
            if(address == null || !address.UserId.Equals(userInfo.Id))
                return NotFound();

            _mapper.Map(request, address);
            _addressRepository.EditAddress(address);
            
            return Ok();
        }
        
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);

            if (userInfo == null || _addressRepository.GetAddress(id).UserId != userInfo.Id)
                return Unauthorized();

            _addressRepository.DeleteAddress(id);
            return Ok();
        }
    }
}