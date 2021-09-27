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

        public async Task<FeedBack> GetById(int id)
        {
            return await _publicRepository.GetFeedBackRepository.GetById(id);
        }

        public void Insert(FeedBack feedBack)
        {
            _publicRepository.GetFeedBackRepository.Insert(feedBack);
        }

        public bool Exists(int id)
        {
            return _publicRepository.GetFeedBackRepository.Exists(id);
        }

        internal void Update(FeedBack feedBack, FeedBack oldFeedBack)
        {
            _publicRepository.GetFeedBackRepository.Update(feedBack, oldFeedBack);
        }

        internal void Delete(FeedBack feedBack)
        {
            _publicRepository.GetFeedBackRepository.Delete(feedBack);
        }

    }
}