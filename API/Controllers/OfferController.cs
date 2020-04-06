using System.Threading.Tasks;
using API.Entities;
using API.Repositories.Interfaces;
using API.Services.User;
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
        private readonly ICurrentUserInfo _currentUserInfo;

        public OfferController(IUserRepository userRepository,
                               IProductRepository productRepository,
                               IOfferRepository offerRepository,
                               ICurrentUserInfo currentUserInfo)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _offerRepository = offerRepository;
            _currentUserInfo = currentUserInfo;
        }


        [HttpPost("{productToOfferId}")]
        public async Task<IActionResult> AddOffer([FromRoute] int productId, int productToOfferId)
        {
            var userId = await _currentUserInfo.GetCurrentUserId();

            var product = _productRepository.FindProductById(productId);
            var productToOffer = _productRepository.FindProductById(productToOfferId);

            if (product == null || productToOffer == null)
                return NotFound();

            if (product.UserId.Equals(userId) || !productToOffer.UserId.Equals(userId))
                return BadRequest();
            
            var offer = new Offer { OfferedProductId = productToOfferId, ReceiverProductId = productId};
            _offerRepository.AddOffer(offer);
            return Ok();
        }
    }
}