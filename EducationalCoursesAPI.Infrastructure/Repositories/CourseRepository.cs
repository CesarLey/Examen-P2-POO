using EducationalCoursesAPI.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace EducationalCoursesAPI.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly string _connectionString;
        public CourseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, title, description, is_published FROM courses WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Course
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    IsPublished = reader.GetBoolean(3)
                };
            }
            return null;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, title, description, is_published FROM courses", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            var courses = new List<Course>();
            while (await reader.ReadAsync())
            {
                var course = new Course
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    IsPublished = reader.GetBoolean(3)
                };
                Console.WriteLine($"Curso: {course.Id}, {course.Title}, IsPublished: {course.IsPublished}");
                courses.Add(course);
            }
            Console.WriteLine($"Total cursos leídos: {courses.Count}");
            return courses;
        }

        public async Task AddAsync(Course course)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("INSERT INTO courses (title, description, is_published) VALUES (@title, @description, @is_published)", conn);
            cmd.Parameters.AddWithValue("@title", course.Title);
            cmd.Parameters.AddWithValue("@description", course.Description);
            cmd.Parameters.AddWithValue("@is_published", course.IsPublished);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Course course)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("UPDATE courses SET title = @title, description = @description, is_published = @is_published WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", course.Id);
            cmd.Parameters.AddWithValue("@title", course.Title);
            cmd.Parameters.AddWithValue("@description", course.Description);
            cmd.Parameters.AddWithValue("@is_published", course.IsPublished);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("DELETE FROM courses WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
        }
    }
} 