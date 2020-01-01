using System.Linq;
using API.Entities;
using API.Repositories.Interfaces;
using API.Services.Database;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Implementations
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly TruequeLibreDbContext _dbContext;

        public QuestionRepository(TruequeLibreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void AddQuestion(Question question)
        {
            _dbContext.Questions.Add(question);
            _dbContext.SaveChanges();
        }

        public void UpdateQuestion(Question question)
        {
            _dbContext.Questions.Update(question);
            _dbContext.SaveChanges();
        }

        public void DeleteQuestionById(int id)
        {
            _dbContext.Questions.Remove(FindQuestionById(id));
            _dbContext.SaveChanges();
        }

        public Question FindQuestionById(int id)
        {
            return _dbContext.Questions.Where(x => x.Id == id)
                .Include(x => x.Product).FirstOrDefault();
            
        }

        public void FindQuestionsByProductId(int id)
        {
            throw new System.NotImplementedException();
        }

        public void FindQuestionsByUserId(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}