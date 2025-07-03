using EducationalCoursesAPI.Domain.Entities;

namespace EducationalCoursesAPI.Infrastructure.Repositories
{
    public interface IModuleRepository
    {
        Task<Module?> GetByIdAsync(int id);
        Task<IEnumerable<Module>> GetAllAsync();
        Task AddAsync(Module module);
        Task UpdateAsync(Module module);
        Task DeleteAsync(int id);
    }
} 