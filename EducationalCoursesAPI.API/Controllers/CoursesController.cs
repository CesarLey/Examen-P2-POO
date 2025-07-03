using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EducationalCoursesAPI.Application.Services;
using EducationalCoursesAPI.Domain.Entities;

namespace EducationalCoursesAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _courseService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _courseService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Course course)
        {
            var ok = await _courseService.AddAsync(course);
            if (!ok) return BadRequest("Datos inválidos.");
            return Ok(course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Course course)
        {
            course.Id = id;
            var ok = await _courseService.UpdateAsync(course);
            if (!ok) return BadRequest("No se puede modificar un curso publicado o no existe.");
            return Ok(course);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _courseService.DeleteAsync(id);
            if (!ok) return BadRequest("No se puede eliminar un curso publicado o no existe.");
            return Ok();
        }
    }
} 