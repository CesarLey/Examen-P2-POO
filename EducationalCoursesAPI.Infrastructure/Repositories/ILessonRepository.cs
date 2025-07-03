using EducationalCoursesAPI.Domain.Entities;

namespace EducationalCoursesAPI.Infrastructure.Repositories
{
    public interface ILessonRepository
    {
        Task<Lesson?> GetByIdAsync(int id);
        Task<IEnumerable<Lesson>> GetAllAsync();
        Task AddAsync(Lesson lesson);
        Task UpdateAsync(Lesson lesson);
        Task DeleteAsync(int id);
    }
} 