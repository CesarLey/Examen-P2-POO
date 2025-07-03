using EducationalCoursesAPI.Domain.Entities;

namespace EducationalCoursesAPI.Application.Services
{
    public interface ILessonService
    {
        Task<Lesson?> GetByIdAsync(int id);
        Task<IEnumerable<Lesson>> GetAllAsync();
        Task<bool> AddAsync(Lesson lesson);
        Task<bool> UpdateAsync(Lesson lesson);
        Task<bool> DeleteAsync(int id);
    }
} 