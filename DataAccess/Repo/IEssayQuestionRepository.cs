using System.Threading.Tasks;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.DataAccess.Repo
{
    public interface IEssayQuestionRepository
    {
        public Task<EssayQuestion> GetById(int id);

        public void Insert(EssayQuestion essayQuestion);

        public void Delete(EssayQuestion essayQuestion);

        public bool Exists(int id);

        public void Update(EssayQuestion essayQuestion, EssayQuestion oldEssayQuestion);

        public void Update(EssayQuestion essayQuestion);
        public void SubmitAnswers(EssayQuestion essayQuestion);
    }
}