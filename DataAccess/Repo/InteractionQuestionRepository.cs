using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using iread_assignment_ms.DataAccess.Data;
using iread_assignment_ms.DataAccess.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace iread_assignment_ms.DataAccess.Repo
{
    public class InteractionQuestionRepository : IInteractionQuestionRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public InteractionQuestionRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InteractionQuestion> GetById(int id)
        {
            return await _context.InteractionQuestion.SingleOrDefaultAsync(m => m.QuestionId == id);
        }

        public void Insert(InteractionQuestion interactionQuestion)
        {
            _context.InteractionQuestion.Add(interactionQuestion);
            _context.SaveChanges();
        }

        public void Delete(InteractionQuestion interactionQuestion)
        {
            _context.InteractionQuestion.Remove(interactionQuestion);
            _context.SaveChanges();
        }

        public void Update(InteractionQuestion interactionQuestion, InteractionQuestion oldInteractionQuestion)
        {
            _context.Entry(oldInteractionQuestion).State = EntityState.Deleted;
            _context.InteractionQuestion.Attach(interactionQuestion);
            _context.Entry(interactionQuestion).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Update(InteractionQuestion interactionQuestion)
        {
            _context.Entry(interactionQuestion).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _context.InteractionQuestion.Any(r => r.QuestionId == id);
        }

    }
}