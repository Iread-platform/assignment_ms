using System.Threading.Tasks;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.DataAccess.Repo
{
    public interface IMultiChoiceAnswerRepository
    {
        public Task<MultiChoiceAnswer> GetById(int id, bool withQuestion);

        public void Insert(MultiChoiceAnswer multiChoiceAnswer);

        public void Delete(MultiChoiceAnswer multiChoiceAnswer);

        public bool Exists(int id);

        public void Update(MultiChoiceAnswer multiChoiceAnswer, MultiChoiceAnswer oldMultiChoiceAnswer);

        public void Update(MultiChoiceAnswer multiChoiceAnswer);
    }
}