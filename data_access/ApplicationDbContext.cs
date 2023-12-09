using LSMS.Models;
using Microsoft.EntityFrameworkCore;

namespace LSMS.data_access
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Course-Professor
            modelBuilder.Entity<Course>()
            .HasMany(p => p.Professors)
            .WithMany(p => p.Courses)
            .UsingEntity<CourseProfessor>(
                 j => j
                .HasOne(j => j.Professor)
                .WithMany(t => t.CourseProfessors)
                .HasForeignKey(t => t.ProfessorId),
                j => j
                .HasOne(j => j.Course)
                .WithMany(t => t.CourseProfessors)
                .HasForeignKey(t => t.CourseId),
                j =>
                {
                    j.HasKey(t => t.Id);
                }
             );
            // CourseProfessor-Student
            modelBuilder.Entity<Student>()
            .HasMany(p => p.CourseProfessors)
            .WithMany(p => p.Students)
            .UsingEntity<Enrollment>(
                 j => j
                .HasOne(j => j.CourseProfessor)
                .WithMany(t => t.Enrollments)
                .HasForeignKey(t => t.CourseProfessorId),
                j => j
                .HasOne(j => j.Student)
                .WithMany(t => t.Enrollments)
                .HasForeignKey(t => t.StudentId),
                j =>
                {
                    j.HasKey(t => new { t.StudentId, t.CourseProfessorId });
                }
             );
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<CourseProfessor> CourseProfessors { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}
