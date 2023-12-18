using LSMS.Models;
using Microsoft.EntityFrameworkCore;

namespace LSMS.data_access
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Course-Professor
            modelBuilder.Entity<Course>()
            .HasMany(p => p.Professors)
            .WithMany(p => p.Courses)
            .UsingEntity<Lecture>(
                 j => j
                .HasOne(j => j.Professor)
                .WithMany(t => t.Lectures)
                .HasForeignKey(t => t.ProfessorSSN),
                j => j
                .HasOne(j => j.Course)
                .WithMany(t => t.Lectures)
                .HasForeignKey(t => t.CourseId),
                j =>
                {
                    j.HasKey(t => t.Id);
                }
             );
            // CourseProfessor-Student
            modelBuilder.Entity<Student>()
            .HasMany(p => p.Lectures)
            .WithMany(p => p.Students)
            .UsingEntity<Enrollment>(
                 j => j
                .HasOne(j => j.Lecture)
                .WithMany(t => t.Enrollments)
                .HasForeignKey(t => t.LectureId),
                j => j
                .HasOne(j => j.Student)
                .WithMany(t => t.Enrollments)
                .HasForeignKey(t => t.StudentSSN),
                j =>
                {
                    j.HasKey(t => new { t.StudentSSN, t.LectureId });
                }
             );
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
    }
}
