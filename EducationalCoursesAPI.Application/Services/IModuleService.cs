using EducationalCoursesAPI.Domain.Entities;

namespace EducationalCoursesAPI.Application.Services
{
    public interface IModuleService
    {
        Task<Module?> GetByIdAsync(int id);
        Task<IEnumerable<Module>> GetAllAsync();
        Task<bool> AddAsync(Module module);
        Task<bool> UpdateAsync(Module module);
        Task<bool> DeleteAsync(int id);
        Task<bool> AddLessonAsync(int moduleId, Lesson lesson);
    }
} 