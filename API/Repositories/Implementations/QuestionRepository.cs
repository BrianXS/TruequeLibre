using System.Linq;
using API.Entities;
using API.Repositories.Interfaces;
using API.Services.Database;

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
        }

        public void DeleteQuestionById(int id)
        {
            _dbContext.Questions.Remove(FindQuestionById(id));
        }

        public Question FindQuestionById(int id)
        {
            return _dbContext.Questions.FirstOrDefault(x => x.Id == id);
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