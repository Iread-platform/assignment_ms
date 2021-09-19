using System.Threading.Tasks;
using iread_assignment_ms.DataAccess;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.Web.Service
{
    public class InteractionQuestionService
    {
        private readonly IPublicRepository _publicRepository;

        public InteractionQuestionService(IPublicRepository publicRepository)
        {
            _publicRepository = publicRepository;
        }

        public async Task<InteractionQuestion> GetById(int id)
        {
            return await _publicRepository.GetInteractionQuestionRepository.GetById(id);
        }

        public void Insert(InteractionQuestion interactionQuestion)
        {
            _publicRepository.GetInteractionQuestionRepository.Insert(interactionQuestion);
        }

        public bool Exists(int id)
        {
            return _publicRepository.GetInteractionQuestionRepository.Exists(id);
        }

        internal void Update(InteractionQuestion interactionQuestion, InteractionQuestion oldInteractionQuestion)
        {
            _publicRepository.GetInteractionQuestionRepository.Update(interactionQuestion, oldInteractionQuestion);
        }

        internal void Delete(InteractionQuestion interactionQuestion)
        {
            _publicRepository.GetInteractionQuestionRepository.Delete(interactionQuestion);
        }
    }
}