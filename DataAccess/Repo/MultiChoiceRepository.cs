using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using iread_assignment_ms.DataAccess.Data;
using iread_assignment_ms.DataAccess.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace iread_assignment_ms.DataAccess.Repo
{
    public class MultiChoiceRepository : IMultiChoiceRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public MultiChoiceRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MultiChoice> GetById(int id)
        {
            return await _context.MultiChoice.SingleOrDefaultAsync(m => m.QuestionId == id);
        }

        public void Insert(MultiChoice multiChoice)
        {
            _context.MultiChoice.Add(multiChoice);
            _context.SaveChanges();
        }

        public void Delete(MultiChoice multiChoice)
        {
            _context.MultiChoice.Remove(multiChoice);
            _context.SaveChanges();
        }

        public void Update(MultiChoice multiChoice, MultiChoice oldMultiChoice)
        {
            _context.Entry(oldMultiChoice).State = EntityState.Deleted;
            _context.MultiChoice.Attach(multiChoice);
            _context.Entry(multiChoice).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Update(MultiChoice multiChoice)
        {
            _context.Entry(multiChoice).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _context.MultiChoice.Any(r => r.AssignmentId == id);
        }

    }
}