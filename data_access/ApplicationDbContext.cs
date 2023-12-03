using LSMS.Models;
using Microsoft.EntityFrameworkCore;

namespace LSMS.data_access
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasMany(p => p.Groups)
                .WithMany(p => p.Students)
                .UsingEntity<StudentGroup>(
                    j => j
                    .HasOne(j => j.Group)
                    .WithMany(t => t.StudentGroups)
                    .HasForeignKey(t => t.GroupId),
                    j => j
                    .HasOne(j => j.Student)
                    .WithMany(t => t.StudentGroups)
                    .HasForeignKey(t => t.StudentId),
                    j =>
                    {
                        j.HasKey(t => new { t.GroupId, t.StudentId });
                    }
                );

            modelBuilder.Entity<Student>()
                .HasMany(p => p.Sections)
                .WithMany(p => p.Students)
                .UsingEntity<StudentSection>(
                    j => j
                    .HasOne(j => j.Section)
                    .WithMany(t => t.StudentSections)
                    .HasForeignKey(t => t.SectionId),
                    j => j
                    .HasOne(j => j.Student)
                    .WithMany(t => t.StudentSections)
                    .HasForeignKey(t => t.StudentId),
                    j =>
                    {
                        j.HasKey(t => new { t.SectionId, t.StudentId });
                    }
                );


            modelBuilder.Entity<Course>()
            .HasMany(p => p.professors)
            .WithMany(p => p.Courses)
            .UsingEntity<CourseProfessor>(
                 j => j
                .HasOne(j => j.Professors)
                .WithMany(t => t.CourseProfessors)
                .HasForeignKey(t => t.ProfessorId),
                j => j
                .HasOne(j => j.Courses)
                .WithMany(t => t.CourseProfessors)
                .HasForeignKey(t => t.CourseId),
                j =>
                {
                    j.HasKey(t => new { t.CourseId, t.ProfessorId });
                }
                );


            modelBuilder.Entity<Course>()
                .HasMany(p => p.teachingAssistants)
                .WithMany(p => p.courses)
                .UsingEntity<CourseTeachingAssistant>(
                   j => j
                   .HasOne(j => j.TeachingAssistants)
                   .WithMany(t => t.courseTeachingAssistants)
                   .HasForeignKey(t => t.teachingAssistantId),
                   j => j
                   .HasOne(j => j.Courses)
                   .WithMany(t => t.courseTeachingAssistants)
                   .HasForeignKey(t => t.CourseId),
                   j =>
                   {
                       j.HasKey(t => new { t.CourseId, t.teachingAssistantId });
                   }
                 );

            modelBuilder.Entity<Student>()
            .HasMany(p => p.Courses)
            .WithMany(c => c.Students)
            .UsingEntity<StudentCourse>
            (
             j => j
             .HasOne(c => c.Courses)
             .WithMany(cp => cp.StudentCourses)
             .HasForeignKey(c => c.CourseId),
             j => j
             .HasOne(m => m.Students)
             .WithMany(cp => cp.StudentCourses)
             .HasForeignKey(p => p.StudentId),
             j =>
             {
                 j.HasKey(t => new { t.StudentId, t.CourseId });
             }
             );

            modelBuilder.Entity<TeachingAssistant>()
         .HasMany(p => p.professors)
         .WithMany(c => c.TeachingAssistants)
         .UsingEntity<ProfessorTeachingAssistant>
         (
             j => j
             .HasOne(m => m.Professors)
             .WithMany(cp => cp.ProfessorTeachingAssistants)
             .HasForeignKey(p => p.ProffessorId),

             j => j
             .HasOne(c => c.TeachingAssistants)
             .WithMany(c => c.professorTeachingAssistants)
             .HasForeignKey(c => c.TeachingAssistantId),
             j =>
             {
                 j.HasKey(t => new { t.TeachingAssistantId, t.ProffessorId });
             }
             );
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<TeachingAssistant> TeachingAssistants { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}
