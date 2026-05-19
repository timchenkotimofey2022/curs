using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentPerformanceApi.DTOs;
using StudentPerformanceApi.Services.Interfaces;

namespace StudentPerformanceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GradesController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public GradesController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<GradeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int? studentId, [FromQuery] int? subjectId)
    {
        var grades = await _gradeService.GetAllAsync(studentId, subjectId);
        return Ok(grades);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GradeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var grade = await _gradeService.GetByIdAsync(id);
        return grade == null ? NotFound() : Ok(grade);
    }

    [HttpGet("group/{groupId}/average")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GroupAverageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGroupAverage(int groupId)
    {
        var average = await _gradeService.GetGroupAverageAsync(groupId);
        return average == null ? NotFound() : Ok(average);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(typeof(GradeDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] GradeCreateDto dto)
    {
        var grade = await _gradeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = grade.Id }, grade);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] GradeUpdateDto dto)
    {
        await _gradeService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _gradeService.DeleteAsync(id);
        return NoContent();
    }
}
