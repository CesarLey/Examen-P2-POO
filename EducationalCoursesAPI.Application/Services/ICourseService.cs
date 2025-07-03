using EducationalCoursesAPI.Domain.Entities;

namespace EducationalCoursesAPI.Application.Services
{
    public interface ICourseService
    {
        Task<Course?> GetByIdAsync(int id);
        Task<IEnumerable<Course>> GetAllAsync();
        Task<bool> AddAsync(Course course);
        Task<bool> UpdateAsync(Course course);
        Task<bool> DeleteAsync(int id);
        Task<bool> PublishAsync(int courseId);
        Task<bool> AddModuleAsync(int courseId, Module module);
        Task<bool> AddInstructorAsync(int courseId, Instructor instructor);
        Task<bool> RemoveInstructorAsync(int courseId, int instructorId);
    }
} 