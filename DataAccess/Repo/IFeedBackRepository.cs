using System.Threading.Tasks;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.DataAccess.Repo
{
    public interface IEssayFeedBackRepository
    {
        public Task<EssayFeedBack> GetById(int id);

        public void Insert(EssayFeedBack feedBack);

        public void Delete(EssayFeedBack feedBack);

        public bool Exists(int id);

        public void Update(EssayFeedBack feedBack, EssayFeedBack oldFeedBack);

    }
}