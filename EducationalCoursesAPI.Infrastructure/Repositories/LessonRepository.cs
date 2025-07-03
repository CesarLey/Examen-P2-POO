using EducationalCoursesAPI.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace EducationalCoursesAPI.Infrastructure.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly string _connectionString;
        public LessonRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<Lesson>> GetAllAsync()
        {
            var lessons = new List<Lesson>();
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, title, content, module_id FROM lessons", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lessons.Add(new Lesson
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Content = reader.GetString(2),
                    ModuleId = reader.GetInt32(3)
                });
            }
            return lessons;
        }

        // Métodos no implementados para la demo
        public Task<Lesson?> GetByIdAsync(int id) => throw new NotImplementedException();
        public Task AddAsync(Lesson lesson) => throw new NotImplementedException();
        public Task UpdateAsync(Lesson lesson) => throw new NotImplementedException();
        public Task DeleteAsync(int id) => throw new NotImplementedException();
    }
} 