using System.Threading.Tasks;
using API.Entities;

namespace API.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        void AddQuestion(Question question);
        void UpdateQuestion(Question question);
        void DeleteQuestionById(int id);
        Question FindQuestionById(int id);
        void FindQuestionsByProductId(int id);
        void FindQuestionsByUserId(int id);
    }
}