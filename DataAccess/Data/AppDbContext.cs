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
            modelBuilder.Entity<Assignment>()
                .HasMany(a => a.Attachments)
                .WithOne(a => a.Assignment)
                .OnDelete(DeleteBehavior.Cascade);
        }

        //entities
        public DbSet<Assignment> Assignments { set; get; }
        public DbSet<AssignmentStatus> AssignmentStatus { set; get; }
        public DbSet<AssignmentStory> AssignmentStory { set; get; }
        public DbSet<Question> Question { set; get; }
        public DbSet<Choice> Choice { set; get; }
        public DbSet<MultiChoice> MultiChoice { set; get; }
        public DbSet<AssignmentAttachment> AssignmentAttachments { set; get; }

        public DbSet<EssayQuestion> EssayQuestion { set; get; }
        public DbSet<InteractionQuestion> InteractionQuestion { set; get; }
        public DbSet<Answer> Answer { set; get; }
        public DbSet<EssayAnswer> EssayAnswer { set; get; }
        public DbSet<MultiChoiceAnswer> MultiChoiceAnswer { set; get; }



    }
}