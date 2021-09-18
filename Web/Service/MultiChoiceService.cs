using System.Threading.Tasks;
using iread_assignment_ms.DataAccess;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.Web.Service
{
    public class MultiChoiceService
    {
        private readonly IPublicRepository _publicRepository;

        public MultiChoiceService(IPublicRepository publicRepository)
        {
            _publicRepository = publicRepository;
        }

        public async Task<MultiChoice> GetById(int id)
        {
            return await _publicRepository.GetMultiChoiceRepository.GetById(id);
        }

        public void Insert(MultiChoice multiChoice, int RightChoiceIndex)
        {

            _publicRepository.GetMultiChoiceRepository.Insert(multiChoice);
            // set the right choice
            multiChoice.RightChoice = multiChoice.Choices[RightChoiceIndex];
            _publicRepository.GetMultiChoiceRepository.Update(multiChoice);

        }

        public bool Exists(int id)
        {
            return _publicRepository.GetMultiChoiceRepository.Exists(id);
        }

        internal void Update(MultiChoice multiChoice, MultiChoice oldMultiChoice)
        {
            _publicRepository.GetMultiChoiceRepository.Update(multiChoice, oldMultiChoice);
        }

        internal void Delete(MultiChoice multiChoice)
        {
            _publicRepository.GetMultiChoiceRepository.Delete(multiChoice);
        }
    }
}