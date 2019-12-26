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
using Microsoft.AspNetCore.Hosting;
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
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public ProductController(IUserRepository userRepository,
                                 IProductRepository productRepository, 
                                 IMapper mapper,
                                 IWebHostEnvironment environment)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _environment = environment;
        }

        [HttpGet("{id}")]
        public ActionResult<GetProductResponse> GetProduct(int id)
        {
            var response = _mapper.Map<GetProductResponse>(_productRepository.GetProductById(id));
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
    }
}