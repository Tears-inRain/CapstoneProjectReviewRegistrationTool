using CapstoneReview.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace CapstoneReview.Repository.Data.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Lecturer> Lecturers { get; set; } = null!;
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Team> Teams { get; set; } = null!;
    public DbSet<Topic> Topics { get; set; } = null!;
    public DbSet<Slot> Slots { get; set; } = null!;
    public DbSet<SlotLecturer> SlotLecturers { get; set; } = null!;
    public DbSet<SlotTopic> SlotTopics { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Topic - Lecturer (1 - N)
        modelBuilder.Entity<Topic>()
            .HasOne(t => t.Lecturer)
            .WithMany(l => l.InstructedTopics)
            .HasForeignKey(t => t.LecturerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Team - Topic (1 - 1)
        modelBuilder.Entity<Team>()
            .HasOne(t => t.Topic)
            .WithOne(t => t.Team)
            .HasForeignKey<Team>(t => t.TopicId)
            .OnDelete(DeleteBehavior.Restrict);

        // Student - Team (1 - N)
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Team)
            .WithMany(t => t.Students)
            .HasForeignKey(s => s.TeamId)
            .OnDelete(DeleteBehavior.SetNull);

        // Many-to-Many: Slot - Lecturer
        modelBuilder.Entity<SlotLecturer>()
            .HasKey(sl => new { sl.SlotId, sl.LecturerId });

        modelBuilder.Entity<SlotLecturer>()
            .HasOne(sl => sl.Slot)
            .WithMany(s => s.SlotLecturers)
            .HasForeignKey(sl => sl.SlotId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SlotLecturer>()
            .HasOne(sl => sl.Lecturer)
            .WithMany(l => l.SlotLecturers)
            .HasForeignKey(sl => sl.LecturerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Many-to-Many: Slot - Topic
        modelBuilder.Entity<SlotTopic>()
            .HasKey(st => new { st.SlotId, st.TopicId });

        modelBuilder.Entity<SlotTopic>()
            .HasOne(st => st.Slot)
            .WithMany(s => s.SlotTopics)
            .HasForeignKey(st => st.SlotId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SlotTopic>()
            .HasOne(st => st.Topic)
            .WithMany(t => t.SlotTopics)
            .HasForeignKey(st => st.TopicId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
