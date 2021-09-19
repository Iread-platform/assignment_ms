using System.Threading.Tasks;
using iread_assignment_ms.DataAccess;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.Web.Service
{
    public class EssayQuestionService
    {
        private readonly IPublicRepository _publicRepository;

        public EssayQuestionService(IPublicRepository publicRepository)
        {
            _publicRepository = publicRepository;
        }

        public async Task<EssayQuestion> GetById(int id)
        {
            return await _publicRepository.GetEssayQuestionRepository.GetById(id);
        }

        public void Insert(EssayQuestion essayQuestion)
        {
            _publicRepository.GetEssayQuestionRepository.Insert(essayQuestion);
        }

        public bool Exists(int id)
        {
            return _publicRepository.GetEssayQuestionRepository.Exists(id);
        }

        internal void Update(EssayQuestion essayQuestion, EssayQuestion oldEssayQuestion)
        {
            _publicRepository.GetEssayQuestionRepository.Update(essayQuestion, oldEssayQuestion);
        }

        internal void Delete(EssayQuestion essayQuestion)
        {
            _publicRepository.GetEssayQuestionRepository.Delete(essayQuestion);
        }
    }
}