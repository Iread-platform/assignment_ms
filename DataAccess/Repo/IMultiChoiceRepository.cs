using System.Threading.Tasks;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.DataAccess.Repo
{
    public interface IMultiChoiceRepository
    {
        public Task<MultiChoice> GetById(int id);

        public void Insert(MultiChoice multiChoice);

        public void Delete(MultiChoice multiChoice);

        public bool Exists(int id);

        public void Update(MultiChoice multiChoice, MultiChoice oldMultiChoice);
        public void Update(MultiChoice multiChoice);

    }
}