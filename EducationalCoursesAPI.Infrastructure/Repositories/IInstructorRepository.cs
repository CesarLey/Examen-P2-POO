using EducationalCoursesAPI.Domain.Entities;

namespace EducationalCoursesAPI.Infrastructure.Repositories
{
    public interface IInstructorRepository
    {
        Task<Instructor?> GetByIdAsync(int id);
        Task<IEnumerable<Instructor>> GetAllAsync();
        Task AddAsync(Instructor instructor);
        Task UpdateAsync(Instructor instructor);
        Task DeleteAsync(int id);
        Task<bool> ExistsByEmailAsync(string email);
    }
} 