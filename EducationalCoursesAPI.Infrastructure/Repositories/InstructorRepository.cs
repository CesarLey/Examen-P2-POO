using EducationalCoursesAPI.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace EducationalCoursesAPI.Infrastructure.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly string _connectionString;
        public InstructorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<Instructor>> GetAllAsync()
        {
            var instructors = new List<Instructor>();
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, name, email FROM instructors", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                instructors.Add(new Instructor
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.IsDBNull(2) ? "" : reader.GetString(2)
                });
            }
            return instructors;
        }

        public async Task<Instructor?> GetByIdAsync(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, name, email FROM instructors WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Instructor
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.IsDBNull(2) ? "" : reader.GetString(2)
                };
            }
            return null;
        }

        public async Task AddAsync(Instructor instructor)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("INSERT INTO instructors (name, email) VALUES (@name, @email)", conn);
            cmd.Parameters.AddWithValue("@name", instructor.Name);
            cmd.Parameters.AddWithValue("@email", instructor.Email);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Instructor instructor)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("UPDATE instructors SET name = @name, email = @email WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", instructor.Id);
            cmd.Parameters.AddWithValue("@name", instructor.Name);
            cmd.Parameters.AddWithValue("@email", instructor.Email);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("DELETE FROM instructors WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public Task<bool> ExistsByEmailAsync(string email) => throw new NotImplementedException();
    }
} 