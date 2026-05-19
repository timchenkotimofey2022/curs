using StudentPerformanceApi.DTOs;
using StudentPerformanceApi.Models;
using StudentPerformanceApi.Repositories.Interfaces;
using StudentPerformanceApi.Services.Interfaces;

namespace StudentPerformanceApi.Services;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _teacherRepository;

    public TeacherService(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task<IEnumerable<TeacherDto>> GetAllAsync()
    {
        var teachers = await _teacherRepository.GetAllAsync();
        return teachers.Select(t => new TeacherDto
        {
            Id = t.Id,
            FullName = t.FullName,
            Email = t.Email
        });
    }

    public async Task<TeacherDto?> GetByIdAsync(int id)
    {
        var teacher = await _teacherRepository.GetByIdAsync(id);
        if (teacher == null) return null;

        return new TeacherDto
        {
            Id = teacher.Id,
            FullName = teacher.FullName,
            Email = teacher.Email
        };
    }

    public async Task<TeacherDto> CreateAsync(TeacherCreateDto dto)
    {
        var teacher = new Teacher
        {
            FullName = dto.FullName,
            Email = dto.Email
        };

        var created = await _teacherRepository.AddAsync(teacher);
        return new TeacherDto
        {
            Id = created.Id,
            FullName = created.FullName,
            Email = created.Email
        };
    }

    public async Task UpdateAsync(int id, TeacherUpdateDto dto)
    {
        var teacher = await _teacherRepository.GetByIdAsync(id);
        if (teacher == null) throw new KeyNotFoundException("Преподаватель не найден");

        teacher.FullName = dto.FullName;
        teacher.Email = dto.Email;
        await _teacherRepository.UpdateAsync(teacher);
    }

    public async Task DeleteAsync(int id)
    {
        var teacher = await _teacherRepository.GetByIdAsync(id);
        if (teacher == null) throw new KeyNotFoundException("Преподаватель не найден");
        await _teacherRepository.DeleteAsync(teacher);
    }
}
