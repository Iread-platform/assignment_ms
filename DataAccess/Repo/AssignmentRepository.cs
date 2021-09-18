﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using iread_assignment_ms.DataAccess.Data;
using iread_assignment_ms.DataAccess.Data.Entity;
using iread_assignment_ms.Web.Dto.AssignmentDTO;
using Microsoft.EntityFrameworkCore;

namespace iread_assignment_ms.DataAccess.Repo
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public AssignmentRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Assignment> GetById(int id)
        {
            return await _context.Assignments
            .Include(a => a.MultiChoices)
            .ThenInclude(m => m.Choices)
            .Where(a => a.AssignmentId == id).SingleOrDefaultAsync();
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

        public async Task<List<AssignmenWithStorytDto>> GetByStudent(string studentId)
        {

            return await _context.Assignments
                        .Include(s => s.AssignmentStudents)
                        .Include(s => s.Stories)
                        .Where(s => s.AssignmentStudents.Any(s => s.StudentId == studentId))
                        .Select(r => new AssignmenWithStorytDto()
                        {
                            AssignmentId = r.AssignmentId,
                            Stories = r.Stories != null && r.Stories.Count > 0 ? _mapper.Map<List<AssignmentStoryDto>>(r.Stories) : null,
                            ClassId = r.ClassId,
                            TeacherFirstName = r.TeacherFirstName,
                            TeacherLastName = r.TeacherLastName,
                            TeacherId = r.TeacherId,
                            Status = r.AssignmentStudents.First() != null ? r.AssignmentStudents.First().Value : null,
                            EndDate = r.EndDate,
                            StartDate = r.StartDate
                        })
                        .ToListAsync();


            // return await _context.AssignmentStatus
            // .Where(s => s.StudentId == studentId)
            // .Include(s => s.Assignment)
            // .Select(r => new Assignment() { AssignmentId = r.AssignmentId.Value }
            // ).
            // ToListAsync();

        }
    }
}