using System.Linq;
using System.Threading.Tasks;
using iread_assignment_ms.DataAccess.Data;
using iread_assignment_ms.DataAccess.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace iread_assignment_ms.DataAccess.Repo
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly AppDbContext _context;

        public AttachmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AssignmentAttachment> GetById(int id)
        {
            return await _context.AssignmentAttachments
                .Include(a => a.Assignment)
                .Where(a => a.AssignmentAttachmentId == id)
                .SingleOrDefaultAsync();
        }

        public void Insert(AssignmentAttachment assignmentAttachment)
        {
            _context.AssignmentAttachments.Add(assignmentAttachment);
            _context.SaveChanges();
        }

        public void Delete(AssignmentAttachment assignmentAttachment)
        {
            _context.AssignmentAttachments.Remove(assignmentAttachment);
            _context.SaveChanges();
        }

        public bool Exists(int id)
        {
           return _context.AssignmentAttachments.Any(a => a.AssignmentAttachmentId == id);
        }

        public void Update(AssignmentAttachment assignmentAttachment, AssignmentAttachment oldAssignmentAttachment)
        {
            throw new System.NotImplementedException();
        }

        public void Update(AssignmentAttachment assignmentAttachment)
        {
            throw new System.NotImplementedException();
        }
    }
}