using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using iread_assignment_ms.DataAccess.Data;
using iread_assignment_ms.DataAccess.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace iread_assignment_ms.DataAccess.Repo
{
    public class EssayFeedBackRepository : IEssayFeedBackRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public EssayFeedBackRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EssayFeedBack> GetById(int id)
        {
            return await _context.EssayFeedBack
            .Where(a => a.FeedBackId == id).SingleOrDefaultAsync();
        }

        public void Insert(EssayFeedBack feedBack)
        {
            _context.EssayFeedBack.Add(feedBack);
            _context.SaveChanges();
        }

        public void Delete(EssayFeedBack feedBack)
        {
            _context.EssayFeedBack.Remove(feedBack);
            _context.SaveChanges();
        }

        public void Update(EssayFeedBack feedBack, EssayFeedBack oldFeedBack)
        {
            _context.Entry(oldFeedBack).State = EntityState.Deleted;
            _context.EssayFeedBack.Attach(feedBack);
            _context.Entry(feedBack).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _context.EssayFeedBack.Any(r => r.FeedBackId == id);
        }

    }
}