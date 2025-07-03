using EducationalCoursesAPI.Domain.Entities;
using EducationalCoursesAPI.Infrastructure.Repositories;

namespace EducationalCoursesAPI.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IInstructorRepository _instructorRepository;

        public CourseService(ICourseRepository courseRepository, IModuleRepository moduleRepository, IInstructorRepository instructorRepository)
        {
            _courseRepository = courseRepository;
            _moduleRepository = moduleRepository;
            _instructorRepository = instructorRepository;
        }

        public async Task<Course?> GetByIdAsync(int id) => await _courseRepository.GetByIdAsync(id);
        public async Task<IEnumerable<Course>> GetAllAsync() => await _courseRepository.GetAllAsync();

        public async Task<bool> AddAsync(Course course)
        {
            if (string.IsNullOrWhiteSpace(course.Title)) return false;
            await _courseRepository.AddAsync(course);
            return true;
        }

        public async Task<bool> UpdateAsync(Course course)
        {
            var existing = await _courseRepository.GetByIdAsync(course.Id);
            if (existing == null || existing.IsPublished) return false;
            existing.Title = course.Title;
            existing.Description = course.Description;
            await _courseRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _courseRepository.GetByIdAsync(id);
            if (existing == null || existing.IsPublished) return false;
            await _courseRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> PublishAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null || course.IsPublished) return false;
            course.IsPublished = true;
            await _courseRepository.UpdateAsync(course);
            return true;
        }

        public async Task<bool> AddModuleAsync(int courseId, Module module)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null || course.IsPublished) return false;
            if (string.IsNullOrWhiteSpace(module.Title)) return false;
            module.CourseId = courseId;
            await _moduleRepository.AddAsync(module);
            return true;
        }

        public async Task<bool> AddInstructorAsync(int courseId, Instructor instructor)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null || course.IsPublished) return false;
            if (await _instructorRepository.ExistsByEmailAsync(instructor.Email)) return false;
            await _instructorRepository.AddAsync(instructor);
            course.Instructors.Add(instructor);
            await _courseRepository.UpdateAsync(course);
            return true;
        }

        public async Task<bool> RemoveInstructorAsync(int courseId, int instructorId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null || course.IsPublished) return false;
            var instructor = course.Instructors.FirstOrDefault(i => i.Id == instructorId);
            if (instructor == null) return false;
            course.Instructors.Remove(instructor);
            await _courseRepository.UpdateAsync(course);
            return true;
        }
    }
} 