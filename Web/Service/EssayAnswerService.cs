using System.Threading.Tasks;
using iread_assignment_ms.DataAccess;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.Web.Service
{
    public class EssayAnswerService
    {
        private readonly IPublicRepository _publicRepository;

        public EssayAnswerService(IPublicRepository publicRepository)
        {
            _publicRepository = publicRepository;
        }

        public async Task<EssayAnswer> GetById(int id, bool withQuestion)
        {
            return await _publicRepository.GetEssayAnswerRepository.GetById(id, withQuestion);
        }

        public void Insert(EssayAnswer essayAnswer)
        {
            _publicRepository.GetEssayAnswerRepository.Insert(essayAnswer);
        }

        public bool Exists(int id)
        {
            return _publicRepository.GetEssayAnswerRepository.Exists(id);
        }

        internal void Update(EssayAnswer essayAnswer, EssayAnswer oldEssayAnswer)
        {
            _publicRepository.GetEssayAnswerRepository.Update(essayAnswer, oldEssayAnswer);
        }
        internal void Update(EssayAnswer essayAnswer)
        {
            _publicRepository.GetEssayAnswerRepository.Update(essayAnswer);
        }

        internal void Delete(EssayAnswer essayAnswer)
        {
            _publicRepository.GetEssayAnswerRepository.Delete(essayAnswer);
        }
    }
}