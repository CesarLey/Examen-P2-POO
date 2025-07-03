using EducationalCoursesAPI.Domain.Entities;
using EducationalCoursesAPI.Infrastructure.Repositories;

namespace EducationalCoursesAPI.Application.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly ICourseRepository _courseRepository;

        public InstructorService(IInstructorRepository instructorRepository, ICourseRepository courseRepository)
        {
            _instructorRepository = instructorRepository;
            _courseRepository = courseRepository;
        }

        public async Task<Instructor?> GetByIdAsync(int id) => await _instructorRepository.GetByIdAsync(id);
        public async Task<IEnumerable<Instructor>> GetAllAsync() => await _instructorRepository.GetAllAsync();

        public async Task<bool> AddAsync(Instructor instructor)
        {
            if (string.IsNullOrWhiteSpace(instructor.Name) || string.IsNullOrWhiteSpace(instructor.Email)) return false;
            if (await _instructorRepository.ExistsByEmailAsync(instructor.Email)) return false;
            await _instructorRepository.AddAsync(instructor);
            return true;
        }

        public async Task<bool> UpdateAsync(Instructor instructor)
        {
            var existing = await _instructorRepository.GetByIdAsync(instructor.Id);
            if (existing == null) return false;
            existing.Name = instructor.Name;
            existing.Email = instructor.Email;
            await _instructorRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null) return false;
            // No eliminar si está en un curso publicado
            foreach (var course in instructor.Courses)
            {
                var c = await _courseRepository.GetByIdAsync(course.Id);
                if (c != null && c.IsPublished)
                    return false;
            }
            await _instructorRepository.DeleteAsync(id);
            return true;
        }
    }
} 