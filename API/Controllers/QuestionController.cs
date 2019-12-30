using System.Net;
using API.Entities;
using API.Repositories.Interfaces;
using API.Resources.Incoming;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class QuestionController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public QuestionController(IUserRepository userRepository,
                                IProductRepository productRepository,
                                IQuestionRepository questionRepository,
                                IMapper mapper)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        [HttpPost("{id}")]
        public IActionResult AddQuestionToProduct(int id, AddQuestionToProduct request)
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = _userRepository.FindUserByName(userName);
            var product = _productRepository.FindProductById(id);

            if (userInfo == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest();

            if (product == null)
                return NotFound();

            if (product.UserId == userInfo.Id)
                return Forbid();


            var question = _mapper.Map<Question>(request);
            question.ProductId = product.Id;
            _questionRepository.AddQuestion(question);
            return Ok();
        }
    }
}