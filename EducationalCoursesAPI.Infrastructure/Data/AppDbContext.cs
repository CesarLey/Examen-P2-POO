using Microsoft.EntityFrameworkCore;
using EducationalCoursesAPI.Domain.Entities;

namespace EducationalCoursesAPI.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Instructor> Instructors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuración de nombres de tablas en minúsculas
            modelBuilder.Entity<Course>().ToTable("courses");
            modelBuilder.Entity<Module>().ToTable("modules");
            modelBuilder.Entity<Lesson>().ToTable("lessons");
            modelBuilder.Entity<Instructor>().ToTable("instructors");
            // Relaciones y restricciones básicas
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Modules)
                .WithOne(m => m.Course!)
                .HasForeignKey(m => m.CourseId);

            modelBuilder.Entity<Module>()
                .HasMany(m => m.Lessons)
                .WithOne(l => l.Module!)
                .HasForeignKey(l => l.ModuleId);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors)
                .WithMany(i => i.Courses)
                .UsingEntity(j => j.ToTable("courseinstructor"));

            // Configuración de nombres de columnas en minúsculas para las claves primarias
            modelBuilder.Entity<Course>().Property(c => c.Id).HasColumnName("id");
            modelBuilder.Entity<Module>().Property(m => m.Id).HasColumnName("id");
            modelBuilder.Entity<Lesson>().Property(l => l.Id).HasColumnName("id");
            modelBuilder.Entity<Instructor>().Property(i => i.Id).HasColumnName("id");
            modelBuilder.Entity<Lesson>().Property(l => l.Content).HasColumnName("content");
        }
    }
} 