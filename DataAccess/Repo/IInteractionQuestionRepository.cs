using System.Threading.Tasks;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.DataAccess.Repo
{
    public interface IInteractionQuestionRepository
    {
        public Task<InteractionQuestion> GetById(int id);

        public void Insert(InteractionQuestion interactionQuestion);

        public void Delete(InteractionQuestion interactionQuestion);

        public bool Exists(int id);

        public void Update(InteractionQuestion interactionQuestion, InteractionQuestion oldInteractionQuestion);

        public void Update(InteractionQuestion interactionQuestion);

    }
}