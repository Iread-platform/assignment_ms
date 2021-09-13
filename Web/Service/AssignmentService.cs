using System.Threading.Tasks;
using iread_assignment_ms.DataAccess;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.Web.Service
{
    public class AssignmentService
    {
        private readonly IPublicRepository _publicRepository;

        public AssignmentService(IPublicRepository publicRepository)
        {
            _publicRepository = publicRepository;
        }

        public async Task<Assignment> GetById(int id)
        {
            return await _publicRepository.getAssignmentRepository.GetById(id);
        }

        public void Insert(Assignment assignment)
        {
            _publicRepository.getAssignmentRepository.Insert(assignment);
        }

        public bool Exists(int id)
        {
            return _publicRepository.getAssignmentRepository.Exists(id);
        }

        internal void Update(Assignment assignment, Assignment oldAssignment)
        {
            _publicRepository.getAssignmentRepository.Update(assignment, oldAssignment);
        }

        internal void Delete(Assignment assignment)
        {
            _publicRepository.getAssignmentRepository.Delete(assignment);
        }

    }
}