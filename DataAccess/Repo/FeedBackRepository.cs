using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using iread_assignment_ms.DataAccess.Data;
using iread_assignment_ms.DataAccess.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace iread_assignment_ms.DataAccess.Repo
{
    public class FeedBackRepository : IFeedBackRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public FeedBackRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<FeedBack> GetById(int id)
        {
            return await _context.FeedBack
            .Where(a => a.FeedBackId == id).SingleOrDefaultAsync();
        }

        public void Insert(FeedBack feedBack)
        {
            _context.FeedBack.Add(feedBack);
            _context.SaveChanges();
        }

        public void Delete(FeedBack feedBack)
        {
            _context.FeedBack.Remove(feedBack);
            _context.SaveChanges();
        }

        public void Update(FeedBack feedBack, FeedBack oldFeedBack)
        {
            _context.Entry(oldFeedBack).State = EntityState.Deleted;
            _context.FeedBack.Attach(feedBack);
            _context.Entry(feedBack).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _context.FeedBack.Any(r => r.FeedBackId == id);
        }

    }
}