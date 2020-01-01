using System.Net;
using System.Threading.Tasks;
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
    [Route("Product/{productId}/[controller]")]
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

        [HttpPost]
        public async Task<IActionResult> AddQuestionToProduct([FromRoute] int productId, AddQuestionToProduct request)
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
            var product = _productRepository.FindProductById(productId);

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
            question.UserId = userInfo.Id;
            _questionRepository.AddQuestion(question);
            return Ok();
        }

        [HttpPost("{questionId}")]
        public async Task<IActionResult> AnswerQuestion(int questionId, AddAnswerRequest request)
        {
            var userName = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(userName);
            var question = _questionRepository.FindQuestionById(questionId);
            
            if (question == null) 
                return NotFound();

            if (userInfo == null || !userInfo.Id.Equals(question.Product.UserId))
                return Unauthorized();

            if (!string.IsNullOrEmpty(question.Answer))
                return UnprocessableEntity();

            question.Answer = request.Answer;
            _questionRepository.UpdateQuestion(question);
            return Ok();
        }

        [HttpDelete("{questionId}")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            var username = HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByName(username);
            var question = _questionRepository.FindQuestionById(questionId);

            if (userInfo == null)
                return Unauthorized();

            if (question == null)
                return NotFound();

            if (!question.UserId.Equals(userInfo.Id) && !question.Product.UserId.Equals(userInfo.Id))
                return UnprocessableEntity();
            
            _questionRepository.DeleteQuestionById(questionId);
            return Ok();
        }
    }
}