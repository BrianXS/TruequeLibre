using System.Threading.Tasks;
using API.Entities;
using API.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("Product/{productId}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OfferController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IMapper _mapper;

        public OfferController(IUserRepository userRepository,
                               IProductRepository productRepository,
                               IOfferRepository offerRepository,
                               IMapper mapper)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _offerRepository = offerRepository;
            _mapper = mapper;
        }


        [HttpPost("{productToOfferId}")]
        public async Task<IActionResult> AddOffer([FromRoute] int productId, int productToOfferId)
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);

            var product = _productRepository.FindProductById(productId);
            var productToOffer = _productRepository.FindProductById(productToOfferId);

            if (userInfo == null)
                return Unauthorized();

            if (product == null || productToOffer == null)
                return NotFound();

            if (product.UserId.Equals(userInfo.Id) || !productToOffer.UserId.Equals(userInfo.Id))
                return BadRequest();
            
            var offer = new Offer { OfferedProductId = productToOfferId, ReceiverProductId = productId};
            _offerRepository.AddOffer(offer);
            return Ok();
        }
    }
}