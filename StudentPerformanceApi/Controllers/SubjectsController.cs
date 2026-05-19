using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentPerformanceApi.DTOs;
using StudentPerformanceApi.Services.Interfaces;

namespace StudentPerformanceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubjectsController : ControllerBase
{
    private readonly ISubjectService _subjectService;

    public SubjectsController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<SubjectDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int? teacherId)
    {
        var subjects = await _subjectService.GetAllAsync(teacherId);
        return Ok(subjects);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SubjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var subject = await _subjectService.GetByIdAsync(id);
        return subject == null ? NotFound() : Ok(subject);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(typeof(SubjectDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] SubjectCreateDto dto)
    {
        var subject = await _subjectService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = subject.Id }, subject);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] SubjectUpdateDto dto)
    {
        await _subjectService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _subjectService.DeleteAsync(id);
        return NoContent();
    }
}
