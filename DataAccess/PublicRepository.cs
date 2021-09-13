
using iread_assignment_ms.DataAccess.Data;
using iread_assignment_ms.DataAccess.Repo;

namespace iread_assignment_ms.DataAccess
{
    public class PublicRepository : IPublicRepository
    {
        private readonly AppDbContext _context;
        private IAssignmentRepository _assignmentRepository;

        public PublicRepository(AppDbContext context)
        {
            _context = context;
        }

        public IAssignmentRepository getAssignmentRepository
        {
            get
            {
                return _assignmentRepository ??= new AssignmentRepository(_context);
            }
        }
    }
}