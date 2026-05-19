using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentPerformanceApi.DTOs;
using StudentPerformanceApi.Services.Interfaces;
using StudentPerformanceApi.Repositories.Interfaces;

namespace StudentPerformanceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupsController : ControllerBase
{
    private readonly IGroupService _groupService;
    private readonly IStudentRepository _studentRepository;

    public GroupsController(IGroupService groupService, IStudentRepository studentRepository)
    {
        _groupService = groupService;
        _studentRepository = studentRepository;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<GroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var groups = await _groupService.GetAllAsync();
        return Ok(groups);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var group = await _groupService.GetByIdAsync(id);
        return group == null ? NotFound() : Ok(group);
    }

    [HttpGet("{id}/students")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<StudentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudents(int id)
    {
        var group = await _groupService.GetByIdAsync(id);
        if (group == null) return NotFound();

        var students = await _studentRepository.FindAsync(s => s.GroupId == id);
        var dtos = students.Select(s => new StudentDto
        {
            Id = s.Id,
            FullName = s.FullName,
            Email = s.Email,
            GroupId = s.GroupId,
            GroupName = group.Name
        });
        return Ok(dtos);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(typeof(GroupDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] GroupCreateDto dto)
    {
        var group = await _groupService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = group.Id }, group);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] GroupUpdateDto dto)
    {
        await _groupService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _groupService.DeleteAsync(id);
        return NoContent();
    }
}
