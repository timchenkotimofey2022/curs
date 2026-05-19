using StudentPerformanceApi.DTOs;
using StudentPerformanceApi.Models;
using StudentPerformanceApi.Repositories.Interfaces;
using StudentPerformanceApi.Services.Interfaces;

namespace StudentPerformanceApi.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IGroupRepository _groupRepository;

    public StudentService(IStudentRepository studentRepository, IGroupRepository groupRepository)
    {
        _studentRepository = studentRepository;
        _groupRepository = groupRepository;
    }

    public async Task<IEnumerable<StudentDto>> GetAllAsync(int? groupId = null)
    {
        var students = groupId.HasValue
            ? await _studentRepository.FindAsync(s => s.GroupId == groupId.Value)
            : await _studentRepository.GetAllAsync();

        return students.Select(s => new StudentDto
        {
            Id = s.Id,
            FullName = s.FullName,
            Email = s.Email,
            GroupId = s.GroupId,
            GroupName = s.Group?.Name ?? string.Empty
        });
    }

    public async Task<StudentDto?> GetByIdAsync(int id)
    {
        var student = await _studentRepository.GetByIdWithDetailsAsync(id);
        if (student == null) return null;

        return new StudentDto
        {
            Id = student.Id,
            FullName = student.FullName,
            Email = student.Email,
            GroupId = student.GroupId,
            GroupName = student.Group?.Name ?? string.Empty
        };
    }

    public async Task<StudentDto> CreateAsync(StudentCreateDto dto)
    {
        if (!await _groupRepository.ExistsAsync(dto.GroupId))
            throw new ArgumentException($"Группа с id={dto.GroupId} не найдена");

        var student = new Student
        {
            FullName = dto.FullName,
            Email = dto.Email,
            GroupId = dto.GroupId
        };

        var created = await _studentRepository.AddAsync(student);
        return new StudentDto
        {
            Id = created.Id,
            FullName = created.FullName,
            Email = created.Email,
            GroupId = created.GroupId,
            GroupName = string.Empty
        };
    }

    public async Task UpdateAsync(int id, StudentUpdateDto dto)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null) throw new KeyNotFoundException("Студент не найден");

        if (!await _groupRepository.ExistsAsync(dto.GroupId))
            throw new ArgumentException($"Группа с id={dto.GroupId} не найдена");

        student.FullName = dto.FullName;
        student.Email = dto.Email;
        student.GroupId = dto.GroupId;
        await _studentRepository.UpdateAsync(student);
    }

    public async Task DeleteAsync(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null) throw new KeyNotFoundException("Студент не найден");
        await _studentRepository.DeleteAsync(student);
    }

    public async Task<StudentPerformanceDto?> GetPerformanceAsync(int studentId)
    {
        var student = await _studentRepository.GetByIdWithDetailsAsync(studentId);
        if (student == null) return null;

        var subjectGrades = student.Grades.Select(g => new SubjectGradeDto
        {
            SubjectId = g.SubjectId,
            SubjectName = g.Subject?.Name ?? string.Empty,
            Value = g.Value,
            Date = g.Date,
            Comment = g.Comment
        }).ToList();

        return new StudentPerformanceDto
        {
            StudentId = student.Id,
            StudentName = student.FullName,
            GroupName = student.Group?.Name ?? string.Empty,
            SubjectGrades = subjectGrades,
            AverageGrade = subjectGrades.Any() ? subjectGrades.Average(sg => sg.Value) : 0
        };
    }
}
