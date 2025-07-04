using EducationalCoursesAPI.Application.Services;
using EducationalCoursesAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalCoursesAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InstructorsController : ControllerBase
    {
        private readonly IInstructorService _instructorService;
        public InstructorsController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _instructorService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _instructorService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Instructor instructor)
        {
            if (string.IsNullOrWhiteSpace(instructor.Name) || string.IsNullOrWhiteSpace(instructor.Email))
                return BadRequest("Datos inválidos.");
            if (await _instructorService.GetAllAsync() is var all && all.Any(i => i.Email == instructor.Email))
                return BadRequest("Ya existe un instructor con ese email.");
            if (all.Any(i => i.Name == instructor.Name))
                return BadRequest("Ya existe un instructor con ese nombre.");
            var ok = await _instructorService.AddAsync(instructor);
            if (!ok) return BadRequest("No se pudo crear el instructor.");
            return Ok(instructor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Instructor instructor)
        {
            instructor.Id = id;
            if (string.IsNullOrWhiteSpace(instructor.Name) || string.IsNullOrWhiteSpace(instructor.Email))
                return BadRequest("Datos inválidos.");
            if (await _instructorService.GetAllAsync() is var all && all.Any(i => i.Id != id && i.Email == instructor.Email))
                return BadRequest("Ya existe un instructor con ese email.");
            if (all.Any(i => i.Id != id && i.Name == instructor.Name))
                return BadRequest("Ya existe un instructor con ese nombre.");
            var ok = await _instructorService.UpdateAsync(instructor);
            if (!ok) return BadRequest("No se puede modificar el instructor o no existe.");
            return Ok(instructor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _instructorService.DeleteAsync(id);
            if (!ok) return BadRequest("No se puede eliminar un instructor de un curso publicado o no existe.");
            return Ok();
        }
    }
} 