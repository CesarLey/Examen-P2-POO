using EducationalCoursesAPI.Domain.Entities;
using EducationalCoursesAPI.Infrastructure.Repositories;

namespace EducationalCoursesAPI.Application.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ILessonRepository _lessonRepository;

        public ModuleService(IModuleRepository moduleRepository, ICourseRepository courseRepository, ILessonRepository lessonRepository)
        {
            _moduleRepository = moduleRepository;
            _courseRepository = courseRepository;
            _lessonRepository = lessonRepository;
        }

        public async Task<Module?> GetByIdAsync(int id) => await _moduleRepository.GetByIdAsync(id);
        public async Task<IEnumerable<Module>> GetAllAsync() => await _moduleRepository.GetAllAsync();

        public async Task<bool> AddAsync(Module module)
        {
            var course = await _courseRepository.GetByIdAsync(module.CourseId);
            if (course == null || course.IsPublished) return false;
            if (string.IsNullOrWhiteSpace(module.Title)) return false;
            await _moduleRepository.AddAsync(module);
            return true;
        }

        public async Task<bool> UpdateAsync(Module module)
        {
            var existing = await _moduleRepository.GetByIdAsync(module.Id);
            var course = existing != null ? await _courseRepository.GetByIdAsync(existing.CourseId) : null;
            if (existing == null || course == null || course.IsPublished) return false;
            existing.Title = module.Title;
            await _moduleRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _moduleRepository.GetByIdAsync(id);
            var course = existing != null ? await _courseRepository.GetByIdAsync(existing.CourseId) : null;
            if (existing == null || course == null || course.IsPublished) return false;
            await _moduleRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> AddLessonAsync(int moduleId, Lesson lesson)
        {
            var module = await _moduleRepository.GetByIdAsync(moduleId);
            var course = module != null ? await _courseRepository.GetByIdAsync(module.CourseId) : null;
            if (module == null || course == null || course.IsPublished) return false;
            if (string.IsNullOrWhiteSpace(lesson.Title)) return false;
            lesson.ModuleId = moduleId;
            await _lessonRepository.AddAsync(lesson);
            return true;
        }
    }
} 