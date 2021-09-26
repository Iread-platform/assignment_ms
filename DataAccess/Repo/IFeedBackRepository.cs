using System.Threading.Tasks;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.DataAccess.Repo
{
    public interface IFeedBackRepository
    {
        public Task<FeedBack> GetById(int id);

        public void Insert(FeedBack feedBack);

        public void Delete(FeedBack feedBack);

        public bool Exists(int id);

        public void Update(FeedBack feedBack, FeedBack oldFeedBack);

    }
}