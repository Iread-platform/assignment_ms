using iread_assignment_ms.DataAccess.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace iread_assignment_ms.DataAccess.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        //entities
        public DbSet<Assignment> Assignments { set; get; }
        public DbSet<AssignmentStatus> AssignmentStatus { set; get; }
        public DbSet<AssignmentStory> AssignmentStory { set; get; }
        public DbSet<Question> Question { set; get; }
        public DbSet<Choice> Choice { set; get; }
        public DbSet<MultiChoice> MultiChoice { set; get; }



    }
}