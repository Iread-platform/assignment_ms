using System.Threading.Tasks;
using iread_assignment_ms.DataAccess;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.Web.Service
{
    public class InteractionAnswerService
    {
        private readonly IPublicRepository _publicRepository;

        public InteractionAnswerService(IPublicRepository publicRepository)
        {
            _publicRepository = publicRepository;
        }

        public async Task<InteractionAnswer> GetById(int id)
        {
            return await _publicRepository.GetInteractionAnswerRepository.GetById(id);
        }

        public void Insert(InteractionAnswer interactionAnswer)
        {
            _publicRepository.GetInteractionAnswerRepository.Insert(interactionAnswer);
        }

        public bool Exists(int id)
        {
            return _publicRepository.GetInteractionAnswerRepository.Exists(id);
        }

        internal void Update(InteractionAnswer interactionAnswer, InteractionAnswer oldInteractionAnswer)
        {
            _publicRepository.GetInteractionAnswerRepository.Update(interactionAnswer, oldInteractionAnswer);
        }

        internal void Update(InteractionAnswer interactionAnswer)
        {
            _publicRepository.GetInteractionAnswerRepository.Update(interactionAnswer);
        }

        internal void Delete(InteractionAnswer interactionAnswer)
        {
            _publicRepository.GetInteractionAnswerRepository.Delete(interactionAnswer);
        }
    }
}