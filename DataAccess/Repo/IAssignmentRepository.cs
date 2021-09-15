using System.Collections.Generic;
using System.Threading.Tasks;
using iread_assignment_ms.DataAccess.Data.Entity;
using iread_assignment_ms.Web.Dto.AssignmentDTO;

namespace iread_assignment_ms.DataAccess.Repo
{
    public interface IAssignmentRepository
    {
        public Task<Assignment> GetById(int id);

        public void Insert(Assignment assignment);

        public void Delete(Assignment assignment);

        public bool Exists(int id);

        public void Update(Assignment assignment, Assignment oldAssignment);

        public Task<List<Assignment>> GetByTeacher(string teacherId);
        public Task<List<AssignmenWithStorytDto>> GetByStudent(string myId);
    }
}