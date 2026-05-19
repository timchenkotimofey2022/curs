using StudentPerformanceApi.DTOs;
using StudentPerformanceApi.Models;
using StudentPerformanceApi.Repositories.Interfaces;
using StudentPerformanceApi.Services.Interfaces;

namespace StudentPerformanceApi.Services;

public class SubjectService : ISubjectService
{
    private readonly ISubjectRepository _subjectRepository;
    private readonly ITeacherRepository _teacherRepository;

    public SubjectService(ISubjectRepository subjectRepository, ITeacherRepository teacherRepository)
    {
        _subjectRepository = subjectRepository;
        _teacherRepository = teacherRepository;
    }

    public async Task<IEnumerable<SubjectDto>> GetAllAsync(int? teacherId = null)
    {
        var subjects = teacherId.HasValue
            ? await _subjectRepository.GetByTeacherIdWithTeacherAsync(teacherId.Value)
            : await _subjectRepository.GetAllWithTeachersAsync();

        return subjects.Select(s => new SubjectDto
        {
            Id = s.Id,
            Name = s.Name,
            TeacherId = s.TeacherId,
            TeacherName = s.Teacher?.FullName ?? string.Empty
        });
    }

    public async Task<SubjectDto?> GetByIdAsync(int id)
    {
        var subject = await _subjectRepository.GetByIdAsync(id);
        if (subject == null) return null;

        return new SubjectDto
        {
            Id = subject.Id,
            Name = subject.Name,
            TeacherId = subject.TeacherId,
            TeacherName = subject.Teacher?.FullName ?? string.Empty
        };
    }

    public async Task<SubjectDto> CreateAsync(SubjectCreateDto dto)
    {
        if (!await _teacherRepository.ExistsAsync(dto.TeacherId))
            throw new ArgumentException($"Преподаватель с id={dto.TeacherId} не найден");

        var subject = new Subject
        {
            Name = dto.Name,
            TeacherId = dto.TeacherId
        };

        var created = await _subjectRepository.AddAsync(subject);
        return new SubjectDto
        {
            Id = created.Id,
            Name = created.Name,
            TeacherId = created.TeacherId,
            TeacherName = string.Empty
        };
    }

    public async Task UpdateAsync(int id, SubjectUpdateDto dto)
    {
        var subject = await _subjectRepository.GetByIdAsync(id);
        if (subject == null) throw new KeyNotFoundException("Предмет не найден");

        if (!await _teacherRepository.ExistsAsync(dto.TeacherId))
            throw new ArgumentException($"Преподаватель с id={dto.TeacherId} не найден");

        subject.Name = dto.Name;
        subject.TeacherId = dto.TeacherId;
        await _subjectRepository.UpdateAsync(subject);
    }

    public async Task DeleteAsync(int id)
    {
        var subject = await _subjectRepository.GetByIdAsync(id);
        if (subject == null) throw new KeyNotFoundException("Предмет не найден");
        await _subjectRepository.DeleteAsync(subject);
    }
}
