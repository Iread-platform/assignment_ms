using System.Threading.Tasks;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.DataAccess.Repo
{
    public interface IInteractionAnswerRepository
    {
        public Task<InteractionAnswer> GetById(int id);

        public void Insert(InteractionAnswer interactionAnswer);

        public void Delete(InteractionAnswer interactionAnswer);

        public bool Exists(int id);

        public void Update(InteractionAnswer interactionAnswer, InteractionAnswer oldInteractionAnswer);

        public void Update(InteractionAnswer interactionAnswer);
        public void AddInteractionToAnswer(AnswerInteraction answerInteraction);
        public void RemoveByInteractionAndAnswer(int AnswerId, int interactionId);

    }
}