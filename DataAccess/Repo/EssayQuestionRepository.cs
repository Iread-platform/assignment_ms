using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using iread_assignment_ms.DataAccess.Data;
using iread_assignment_ms.DataAccess.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace iread_assignment_ms.DataAccess.Repo
{
    public class EssayQuestionRepository : IEssayQuestionRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public EssayQuestionRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EssayQuestion> GetById(int id)
        {
            return await _context.EssayQuestion.SingleOrDefaultAsync(m => m.QuestionId == id);
        }

        public void Insert(EssayQuestion essayQuestion)
        {
            _context.EssayQuestion.Add(essayQuestion);
            _context.SaveChanges();
        }

        public void Delete(EssayQuestion essayQuestion)
        {
            _context.EssayQuestion.Remove(essayQuestion);
            _context.SaveChanges();
        }

        public void Update(EssayQuestion essayQuestion, EssayQuestion oldEssayQuestion)
        {
            _context.Entry(oldEssayQuestion).State = EntityState.Deleted;
            _context.EssayQuestion.Attach(essayQuestion);
            _context.Entry(essayQuestion).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Update(EssayQuestion essayQuestion)
        {
            _context.Entry(essayQuestion).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _context.EssayQuestion.Any(r => r.QuestionId == id);
        }

    }
}