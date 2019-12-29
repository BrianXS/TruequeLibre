using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPictureRepository _pictureRepository;
        private readonly IMapper _mapper;

        public ProductController(IUserRepository userRepository,
            IProductRepository productRepository,
            IPictureRepository pictureRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _pictureRepository = pictureRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}"), AllowAnonymous]
        public ActionResult<GetProductResponse> GetProduct(int id)
        {
            var response = _mapper.Map<GetProductResponse>(_productRepository.FindProductById(id));
            if (response == null)
                return NotFound((GetProductResponse) null);
            
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductRequest request)
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
            
            if (!ModelState.IsValid || userInfo == null)
                return BadRequest();

            var result = _mapper.Map<Product>(request);
            result.UserId = userInfo.Id;
            if (request.Pictures.Any())
                request.Pictures.ForEach(x => Base64ToFile.ConvertToFile(x.Content, x.Name));
            
            _productRepository.SaveProduct(result);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);

            if (userInfo == null)
                return BadRequest();

            _productRepository.DeleteProduct(id);
            return Ok();
        }

        [HttpGet("edit/{id}")]
        public async Task<ActionResult<UpdateProductResponse>> UpdateProduct(int id)
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
            var response = _mapper.Map<UpdateProductResponse>(_productRepository.FindProductById(id));

            if (userInfo == null || !userInfo.Id.Equals(_productRepository.FindProductById(id).UserId))
                return Forbid();
                
            return Ok(response);
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductRequest request)
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
            var productToUpdate = _productRepository.FindProductById(id);

            if (userInfo == null) return Unauthorized();
            
            if (productToUpdate == null) return NotFound();
            
            if (!userInfo.Id.Equals(productToUpdate.UserId)) return Forbid();

            if (!ModelState.IsValid) return BadRequest();

            var remaningPictures = productToUpdate.Pictures
                .Select(x => _mapper.Map<UpdateProductRequest.PictureDto>(x)).ToList();

            remaningPictures = remaningPictures.Intersect(request.Pictures)
                .Concat(request.Pictures.Except(remaningPictures)).ToList();

            var picturesNotToDelete = productToUpdate.Pictures
                .Join(remaningPictures,
                    existing => productToUpdate.Pictures.Select(x => x.FileName.StringToBase64()),
                    remaining => request.Pictures.Select(x => x.Content),
                    (existing, remaning) => existing).ToList();
            
            if (!picturesNotToDelete.Count.Equals(request.Pictures.Count))
            {
                var picturesToDelete = productToUpdate.Pictures.Except(picturesNotToDelete).ToList();

                var picturesToSave = _mapper.Map<List<PictureInfo>>(request.Pictures);
                picturesToSave.ForEach(x => x.ProductId = productToUpdate.Id);

                _pictureRepository.DeletePictures(picturesToDelete);
                _pictureRepository.SavePictures(picturesToSave);
                request.Pictures.ForEach(x => Base64ToFile.ConvertToFile(x.Content, x.Name));
            }
            
            return Ok();
        }
    }
}