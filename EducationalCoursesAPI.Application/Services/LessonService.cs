using EducationalCoursesAPI.Domain.Entities;
using EducationalCoursesAPI.Infrastructure.Repositories;

namespace EducationalCoursesAPI.Application.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly ICourseRepository _courseRepository;

        public LessonService(ILessonRepository lessonRepository, IModuleRepository moduleRepository, ICourseRepository courseRepository)
        {
            _lessonRepository = lessonRepository;
            _moduleRepository = moduleRepository;
            _courseRepository = courseRepository;
        }

        public async Task<Lesson?> GetByIdAsync(int id) => await _lessonRepository.GetByIdAsync(id);
        public async Task<IEnumerable<Lesson>> GetAllAsync() => await _lessonRepository.GetAllAsync();

        public async Task<bool> AddAsync(Lesson lesson)
        {
            var module = await _moduleRepository.GetByIdAsync(lesson.ModuleId);
            var course = module != null ? await _courseRepository.GetByIdAsync(module.CourseId) : null;
            if (module == null || course == null || course.IsPublished) return false;
            if (string.IsNullOrWhiteSpace(lesson.Title)) return false;
            await _lessonRepository.AddAsync(lesson);
            return true;
        }

        public async Task<bool> UpdateAsync(Lesson lesson)
        {
            var existing = await _lessonRepository.GetByIdAsync(lesson.Id);
            var module = existing != null ? await _moduleRepository.GetByIdAsync(existing.ModuleId) : null;
            var course = module != null ? await _courseRepository.GetByIdAsync(module.CourseId) : null;
            if (existing == null || module == null || course == null || course.IsPublished) return false;
            existing.Title = lesson.Title;
            existing.Content = lesson.Content;
            await _lessonRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _lessonRepository.GetByIdAsync(id);
            var module = existing != null ? await _moduleRepository.GetByIdAsync(existing.ModuleId) : null;
            var course = module != null ? await _courseRepository.GetByIdAsync(module.CourseId) : null;
            if (existing == null || module == null || course == null || course.IsPublished) return false;
            await _lessonRepository.DeleteAsync(id);
            return true;
        }
    }
} 