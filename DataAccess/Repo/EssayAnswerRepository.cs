using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using iread_assignment_ms.DataAccess.Data;
using iread_assignment_ms.DataAccess.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace iread_assignment_ms.DataAccess.Repo
{
    public class EssayAnswerRepository : IEssayAnswerRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public EssayAnswerRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EssayAnswer> GetById(int id, bool withQuestion)
        {
            return withQuestion ?
             await _context.EssayAnswer.Include(e => e.Question).ThenInclude(q => q.Assignment).SingleOrDefaultAsync(m => m.AnswerId == id) :
             await _context.EssayAnswer.SingleOrDefaultAsync(m => m.AnswerId == id);
        }

        public void Insert(EssayAnswer essayAnswer)
        {
            _context.EssayAnswer.Add(essayAnswer);
            _context.SaveChanges();
        }

        public void Delete(EssayAnswer essayAnswer)
        {
            _context.EssayAnswer.Remove(essayAnswer);
            _context.SaveChanges();
        }

        public void Update(EssayAnswer essayAnswer, EssayAnswer oldEssayAnswer)
        {
            _context.Entry(oldEssayAnswer).State = EntityState.Deleted;
            _context.EssayAnswer.Attach(essayAnswer);
            _context.Entry(essayAnswer).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Update(EssayAnswer essayAnswer)
        {
            _context.Entry(essayAnswer).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _context.EssayAnswer.Any(r => r.AnswerId == id);
        }


    }
}