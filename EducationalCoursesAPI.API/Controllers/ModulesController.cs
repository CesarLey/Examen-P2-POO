using EducationalCoursesAPI.Application.Services;
using EducationalCoursesAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalCoursesAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService _moduleService;
        public ModulesController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _moduleService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _moduleService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Module module)
        {
            var ok = await _moduleService.AddAsync(module);
            if (!ok) return BadRequest("Datos inválidos o curso publicado.");
            return Ok(module);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Module module)
        {
            module.Id = id;
            var ok = await _moduleService.UpdateAsync(module);
            if (!ok) return BadRequest("No se puede modificar un módulo de un curso publicado o no existe.");
            return Ok(module);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _moduleService.DeleteAsync(id);
            if (!ok) return BadRequest("No se puede eliminar un módulo de un curso publicado o no existe.");
            return Ok();
        }

        [HttpPost("{moduleId}/lessons")]
        public async Task<IActionResult> AddLesson(int moduleId, [FromBody] Lesson lesson)
        {
            var ok = await _moduleService.AddLessonAsync(moduleId, lesson);
            if (!ok) return BadRequest("No se puede agregar lección a un módulo de un curso publicado o datos inválidos.");
            return Ok(lesson);
        }
    }
} 