using iread_assignment_ms.DataAccess.Repo;

namespace iread_assignment_ms.DataAccess
{
    public interface IPublicRepository
    {
        IAssignmentRepository getAssignmentRepository { get; }
    }
}