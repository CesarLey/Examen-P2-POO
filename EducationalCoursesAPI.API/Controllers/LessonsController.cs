using EducationalCoursesAPI.Application.Services;
using EducationalCoursesAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalCoursesAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _lessonService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _lessonService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Lesson lesson)
        {
            var ok = await _lessonService.AddAsync(lesson);
            if (!ok) return BadRequest("Datos inválidos o curso publicado.");
            return Ok(lesson);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Lesson lesson)
        {
            lesson.Id = id;
            var ok = await _lessonService.UpdateAsync(lesson);
            if (!ok) return BadRequest("No se puede modificar una lección de un curso publicado o no existe.");
            return Ok(lesson);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _lessonService.DeleteAsync(id);
            if (!ok) return BadRequest("No se puede eliminar una lección de un curso publicado o no existe.");
            return Ok();
        }
    }
} 