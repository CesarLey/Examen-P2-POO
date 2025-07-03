using EducationalCoursesAPI.Domain.Entities;

namespace EducationalCoursesAPI.Application.Services
{
    public interface IInstructorService
    {
        Task<Instructor?> GetByIdAsync(int id);
        Task<IEnumerable<Instructor>> GetAllAsync();
        Task<bool> AddAsync(Instructor instructor);
        Task<bool> UpdateAsync(Instructor instructor);
        Task<bool> DeleteAsync(int id);
    }
} 