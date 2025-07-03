using EducationalCoursesAPI.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace EducationalCoursesAPI.Infrastructure.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly string _connectionString;
        public ModuleRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<Module>> GetAllAsync()
        {
            var modules = new List<Module>();
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, title, course_id FROM modules", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                modules.Add(new Module
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    CourseId = reader.GetInt32(2)
                });
            }
            return modules;
        }

        // Métodos no implementados para la demo
        public Task<Module?> GetByIdAsync(int id) => throw new NotImplementedException();
        public Task AddAsync(Module module) => throw new NotImplementedException();
        public Task UpdateAsync(Module module) => throw new NotImplementedException();
        public Task DeleteAsync(int id) => throw new NotImplementedException();
    }
} 