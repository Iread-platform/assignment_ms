using System.Threading.Tasks;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.DataAccess.Repo
{
    public interface IAttachmentRepository
    {
        public Task<AssignmentAttachment> GetById(int id);

        public void Insert(AssignmentAttachment assignmentAttachment);

        public void Delete(AssignmentAttachment assignmentAttachment);

        public bool Exists(int id);

        public void Update(AssignmentAttachment assignmentAttachment, AssignmentAttachment oldAssignmentAttachment);
        public void Update(AssignmentAttachment assignmentAttachment);

    }
}