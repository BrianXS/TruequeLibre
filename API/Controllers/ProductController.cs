using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Repositories.Implementations;
using API.Repositories.Interfaces;
using API.Resources.Incoming;
using API.Resources.Outgoing;
using API.Services.User;
using API.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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
        private readonly IDetailRepository _detailRepository;
        private readonly ICurrentUserInfo _currentUserInfo;
        private readonly IMapper _mapper;

        public ProductController(IUserRepository userRepository,
            IProductRepository productRepository,
            IPictureRepository pictureRepository,
            IDetailRepository detailRepository,
            ICurrentUserInfo currentUserInfo,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _pictureRepository = pictureRepository;
            _detailRepository = detailRepository;
            _currentUserInfo = currentUserInfo;
            _mapper = mapper;
        }

        [HttpGet("{id}"), AllowAnonymous]
        public ActionResult<GetProductResponse> GetProduct(int id)
        {
            var response = _mapper.Map<GetProductResponse>(_productRepository.FindProductById(id));
            if (response == null)
                return NotFound(null);
            
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductRequest request)
        {
            var userInfo = await _currentUserInfo.GetCurrentUser();
            
            if (!ModelState.IsValid || userInfo == null)
                return BadRequest();

            var result = _mapper.Map<Product>(request);
            result.UserId = userInfo.Id;
            if (request.Pictures.Any())
                request.Pictures.ForEach(x => 
                    Base64ToFile.ConvertToFile(x.Content, x.Name));
            
            _productRepository.SaveProduct(result);
            return Ok();
        }

        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(int productId)
        {
            _productRepository.DeleteProduct(productId);
            return Ok();
        }

        [HttpGet("edit/{id}")]
        public async Task<ActionResult<UpdateProductResponse>> UpdateProduct(int id)
        {
            var userInfo = await _currentUserInfo.GetCurrentUser();
            var response = _mapper.Map<UpdateProductResponse>(_productRepository.FindProductById(id));

            if (userInfo == null || !userInfo.Id.Equals(_productRepository.FindProductById(id).UserId))
                return Forbid();
                
            return Ok(response);
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductRequest request)
        {
            var userInfo = await _currentUserInfo.GetCurrentUser();
            var productToUpdate = _productRepository.FindProductById(id);

            if (userInfo == null) 
                return Unauthorized();
            
            if (productToUpdate == null || !userInfo.Id.Equals(productToUpdate.UserId)) 
                return NotFound();

            if (!ModelState.IsValid) 
                return BadRequest();
            
            var picturesNotToDelete = productToUpdate.Pictures
                .Join(request.Pictures,
                    existing => Base64ToFile.ConvertToBase64(existing.FileName),
                    remaining =>  remaining.Content,
                    (existing, remaning) => existing).ToList();
            
            if (!picturesNotToDelete.Count.Equals(request.Pictures.Count))
            {
                var excludePictures = productToUpdate.Pictures
                    .Select(x => _mapper.Map<UpdateProductRequest.PictureDto>(x)).ToList();

                var picturesToDelete = productToUpdate.Pictures.Except(picturesNotToDelete).ToList();

                var picturesToSave = _mapper.Map<List<PictureInfo>>(request.Pictures.Where(x => !excludePictures.Contains(x)));
                picturesToSave.ForEach(x => x.ProductId = productToUpdate.Id);

                _pictureRepository.DeletePictures(picturesToDelete);
                _pictureRepository.SavePictures(picturesToSave);
                request.Pictures.ForEach(x => Base64ToFile.ConvertToFile(x.Content, x.Name));
            }


            var detailsNotToDelete = productToUpdate.Details
                .Join(request.Details,
                    existing => productToUpdate.Details.Select(x => x.Description),
                    remaining => request.Details.Select(x => x.Description),
                    (existing, remaining) => existing).ToList();

            if (!detailsNotToDelete.Count.Equals(request.Details.Count))
            {
                var excludeDetails = productToUpdate.Details
                    .Select(x => _mapper.Map<UpdateProductRequest.DetailDto>(x)).ToList();
                var detailsToDelete = productToUpdate.Details.Except(detailsNotToDelete).ToList();
                var detailsToSave = _mapper.Map<List<Detail>>(request.Details.Where(x => !excludeDetails.Contains(x)).ToList());
                detailsToSave.ForEach(x => x.ProductId = productToUpdate.Id);
                
                _detailRepository.RemoveRange(detailsToDelete);
                _detailRepository.AddRange(detailsToSave);
            }

            return Ok();
        }
    }
}