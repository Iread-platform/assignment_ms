using System.Threading.Tasks;
using iread_assignment_ms.DataAccess;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.Web.Service
{
    public class FeedBackService
    {
        private readonly IPublicRepository _publicRepository;

        public FeedBackService(IPublicRepository publicRepository)
        {
            _publicRepository = publicRepository;
        }

        public async Task<EssayFeedBack> GetById(int id)
        {
            return await _publicRepository.GetEssayFeedBackRepository.GetById(id);
        }

        public void Insert(EssayFeedBack feedBack)
        {
            _publicRepository.GetEssayFeedBackRepository.Insert(feedBack);
        }

        public bool Exists(int id)
        {
            return _publicRepository.GetEssayFeedBackRepository.Exists(id);
        }

        internal void Update(EssayFeedBack feedBack, EssayFeedBack oldFeedBack)
        {
            _publicRepository.GetEssayFeedBackRepository.Update(feedBack, oldFeedBack);
        }

        internal void Delete(EssayFeedBack feedBack)
        {
            _publicRepository.GetEssayFeedBackRepository.Delete(feedBack);
        }

    }
}