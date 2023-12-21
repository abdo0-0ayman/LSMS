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
            .HasMany(p => p.professors)
            .WithMany(p => p.courses)
            .UsingEntity<Lecture>(
                 j => j
                .HasOne(j => j.professor)
                .WithMany(t => t.lectures)
                .HasForeignKey(t => t.professorSSN),
                j => j
                .HasOne(j => j.course)
                .WithMany(t => t.lectures)
                .HasForeignKey(t => t.courseId),
                j =>
                {
                    j.HasKey(t => t.id);
                }
             );
            // CourseProfessor-Student
            modelBuilder.Entity<Student>()
            .HasMany(p => p.lectures)
            .WithMany(p => p.students)
            .UsingEntity<Enrollment>(
                 j => j
                .HasOne(j => j.lecture)
                .WithMany(t => t.enrollments)
                .HasForeignKey(t => t.lectureId),
                j => j
                .HasOne(j => j.student)
                .WithMany(t => t.enrollments)
                .HasForeignKey(t => t.studentSSN),
                j =>
                {
                    j.HasKey(t => new { t.studentSSN, t.lectureId });
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
       
    }
}
