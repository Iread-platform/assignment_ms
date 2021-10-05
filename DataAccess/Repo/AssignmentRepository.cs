using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using iread_assignment_ms.DataAccess.Data;
using iread_assignment_ms.DataAccess.Data.Entity;
using iread_assignment_ms.DataAccess.Data.Entity.Type;
using iread_assignment_ms.Web.Dto.AssignmentDTO;
using iread_assignment_ms.Web.Dto.AttachmentDto;
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
            .Include(a => a.EssayQuestions)
            .Include(a => a.InteractionQuestions)
            .Include(a => a.AssignmentStatuses)
            .Include(a => a.Attachments)
            .Include(a => a.Stories)
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

        public async Task<List<Assignment>> GetByStudent(string studentId)
        {

            return await _context.Assignments
                        .Include(s => s.AssignmentStatuses)
                        .Include(s => s.Stories)
                        .Include(a => a.Attachments)
                        .Where(s => s.AssignmentStatuses.Any(s => s.StudentId == studentId))
                        .ToListAsync();

        }

        public void SubmitAnswers(int assignmentId, string studentId)
        {
            var studentAnswersList = _context.Answer
                                   .FromSqlRaw($@"SELECT * FROM Answer 
                WHERE StudentId = '{studentId}' AND
                AnswerId in 
                (SELECT AnswerId FROM Answer 
                WHERE EssayQuestionQuestionId in 
                (Select QuestionId FROM Question where AssignmentId = {assignmentId}) 
                or MultiChoiceQuestionId in  
                (Select QuestionId FROM Question where AssignmentId = {assignmentId}) 
                or InteractionQuestionQuestionId in  
                (Select QuestionId FROM Question where AssignmentId = {assignmentId}))")
                                   .ToList<Answer>();

            AssignmentStatus statusAssgiment = _context.AssignmentStatus.Where(ass => ass.AssignmentId == assignmentId && ass.StudentId == studentId).FirstOrDefault();
            statusAssgiment.Value = AssignmentStatusTypes.WaitingForFeedBack.ToString();
            _context.AssignmentStatus.Update(statusAssgiment);

            _context.SaveChanges();
        }

        public bool IsMine(int assignmentId, string studentId)
        {
            return _context.Assignments
            .Include(a => a.EssayQuestions)
                        .ThenInclude(q => q.EssayAnswers)
            .Include(a => a.InteractionQuestions)
                        .ThenInclude(q => q.InteractionAnswers)
            .Include(a => a.MultiChoices)
                        .ThenInclude(q => q.MultiChoiceAnswers)

            .Where(a => a.AssignmentId == assignmentId
            &&
            (
                (a.EssayQuestions.Any(q => q.EssayAnswers.Any(a => a.StudentId == studentId)))
            ||
                (a.InteractionQuestions.Any(q => q.InteractionAnswers.Any(a => a.StudentId == studentId)))
            ||
                (a.MultiChoices.Any(q => q.MultiChoiceAnswers.Any(a => a.StudentId == studentId)))
            )
            ).Any();



        }

        public AssignmentStatus GetStatusByAssignmentStudentId(int assignmentId, string studentId)
        {
            return _context.AssignmentStatus.Include(ass => ass.Assignment).Where(ass => ass.StudentId == studentId
            && ass.AssignmentId == assignmentId).FirstOrDefault();
        }

        public void Update(AssignmentStatus assignmentStatus)
        {
            _context.AssignmentStatus.Update(assignmentStatus);
            _context.SaveChanges();
        }

        public async Task<Question> GetQuestionById(int questionId)
        {
            return await _context.Question.FromSqlRaw(@$"SELECT * 
            FROM Question WHERE QuestionId = {questionId}"
            ).Select(q => new Question()
            {
                AssignmentId = q.AssignmentId,
                QuestionId = q.QuestionId,
                Text = q.Text
            }).
                FirstOrDefaultAsync();
        }
    }
}