using System.Threading.Tasks;
using iread_assignment_ms.DataAccess;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.Web.Service
{
    public class MultiChoiceAnswerService
    {
        private readonly IPublicRepository _publicRepository;

        public MultiChoiceAnswerService(IPublicRepository publicRepository)
        {
            _publicRepository = publicRepository;
        }

        public async Task<MultiChoiceAnswer> GetById(int id, bool withQuestion)
        {
            return await _publicRepository.GetMultiChoiceAnswerRepository.GetById(id, withQuestion);
        }

        public void Insert(MultiChoiceAnswer multiChoiceAnswer)
        {
            _publicRepository.GetMultiChoiceAnswerRepository.Insert(multiChoiceAnswer);
        }

        public bool Exists(int id)
        {
            return _publicRepository.GetMultiChoiceAnswerRepository.Exists(id);
        }

        internal void Update(MultiChoiceAnswer multiChoiceAnswer, MultiChoiceAnswer oldMultiChoiceAnswer)
        {
            _publicRepository.GetMultiChoiceAnswerRepository.Update(multiChoiceAnswer, oldMultiChoiceAnswer);
        }
        internal void Update(MultiChoiceAnswer multiChoiceAnswer)
        {
            _publicRepository.GetMultiChoiceAnswerRepository.Update(multiChoiceAnswer);
        }

        internal void Delete(MultiChoiceAnswer multiChoiceAnswer)
        {
            _publicRepository.GetMultiChoiceAnswerRepository.Delete(multiChoiceAnswer);
        }
    }
}