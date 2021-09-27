using System.Threading.Tasks;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.DataAccess.Repo
{
    public interface IEssayAnswerRepository
    {
        public Task<EssayAnswer> GetById(int id, bool withQuestion);

        public void Insert(EssayAnswer essayAnswer);

        public void Delete(EssayAnswer essayAnswer);

        public bool Exists(int id);

        public void Update(EssayAnswer essayAnswer, EssayAnswer oldEssayAnswer);

        public void Update(EssayAnswer essayAnswer);
    }
}