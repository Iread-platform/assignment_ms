using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using iread_assignment_ms.DataAccess.Data;
using iread_assignment_ms.DataAccess.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace iread_assignment_ms.DataAccess.Repo
{
    public class MultiChoiceAnswerRepository : IMultiChoiceAnswerRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public MultiChoiceAnswerRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MultiChoiceAnswer> GetById(int id, bool withQuestion)
        {
            return withQuestion ?
             await _context.MultiChoiceAnswer
             .Include(e => e.Question)
             .ThenInclude(q => ((MultiChoice)q).Assignment)
             .SingleOrDefaultAsync(m => m.AnswerId == id) :
             await _context.MultiChoiceAnswer.SingleOrDefaultAsync(m => m.AnswerId == id);
        }

        public void Insert(MultiChoiceAnswer multiChoiceAnswer)
        {
            _context.MultiChoiceAnswer.Add(multiChoiceAnswer);
            _context.SaveChanges();
        }

        public void Delete(MultiChoiceAnswer multiChoiceAnswer)
        {
            _context.MultiChoiceAnswer.Remove(multiChoiceAnswer);
            _context.SaveChanges();
        }

        public void Update(MultiChoiceAnswer multiChoiceAnswer, MultiChoiceAnswer oldMultiChoiceAnswer)
        {
            _context.Entry(oldMultiChoiceAnswer).State = EntityState.Deleted;
            _context.MultiChoiceAnswer.Attach(multiChoiceAnswer);
            _context.Entry(multiChoiceAnswer).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Update(MultiChoiceAnswer multiChoiceAnswer)
        {
            _context.Entry(multiChoiceAnswer).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _context.MultiChoiceAnswer.Any(r => r.AnswerId == id);
        }


    }
}