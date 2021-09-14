using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iread_assignment_ms.DataAccess.Data;
using iread_assignment_ms.DataAccess.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace iread_assignment_ms.DataAccess.Repo
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly AppDbContext _context;

        public AssignmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Assignment> GetById(int id)
        {
            return await _context.Assignments.FindAsync(id);
        }

        public async Task<List<Assignment>> GetByTeacher(string teacherId)
        {
            return await _context.Assignments.Where(a => a.TeacherId == teacherId).ToListAsync();
        }


        public void Insert(Assignment assignment)
        {
            _context.Assignments.Add(assignment);
            _context.SaveChanges();
        }

        public void Delete(Assignment assignment)
        {
            _context.Assignments.Remove(assignment);
            _context.SaveChanges();
        }

        public void Update(Assignment assignment, Assignment oldAssignment)
        {
            _context.Entry(oldAssignment).State = EntityState.Deleted;
            _context.Assignments.Attach(assignment);
            _context.Entry(assignment).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _context.Assignments.Any(r => r.AssignmentId == id);
        }

    }
}