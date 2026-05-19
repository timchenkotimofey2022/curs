using StudentPerformanceApi.DTOs;
using StudentPerformanceApi.Models;
using StudentPerformanceApi.Repositories.Interfaces;
using StudentPerformanceApi.Services.Interfaces;

namespace StudentPerformanceApi.Services;

public class GradeService : IGradeService
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ISubjectRepository _subjectRepository;

    public GradeService(IGradeRepository gradeRepository, IGroupRepository groupRepository,
        IStudentRepository studentRepository, ISubjectRepository subjectRepository)
    {
        _gradeRepository = gradeRepository;
        _groupRepository = groupRepository;
        _studentRepository = studentRepository;
        _subjectRepository = subjectRepository;
    }

    public async Task<IEnumerable<GradeDto>> GetAllAsync(int? studentId = null, int? subjectId = null)
    {
        IEnumerable<Grade> grades;
        if (studentId.HasValue && subjectId.HasValue)
            grades = await _gradeRepository.GetByStudentAndSubjectIdAsync(studentId.Value, subjectId.Value);
        else if (studentId.HasValue)
            grades = await _gradeRepository.GetByStudentIdAsync(studentId.Value);
        else if (subjectId.HasValue)
            grades = await _gradeRepository.GetBySubjectIdAsync(subjectId.Value);
        else
            grades = await _gradeRepository.GetAllWithDetailsAsync();

        return grades.Select(MapToDto);
    }

    public async Task<GradeDto?> GetByIdAsync(int id)
    {
        var grade = await _gradeRepository.GetByIdAsync(id);
        return grade == null ? null : MapToDto(grade);
    }

    public async Task<GradeDto> CreateAsync(GradeCreateDto dto)
    {
        if (!await _studentRepository.ExistsAsync(dto.StudentId))
            throw new ArgumentException($"Студент с id={dto.StudentId} не найден");

        if (!await _subjectRepository.ExistsAsync(dto.SubjectId))
            throw new ArgumentException($"Предмет с id={dto.SubjectId} не найден");

        var grade = new Grade
        {
            StudentId = dto.StudentId,
            SubjectId = dto.SubjectId,
            Value = dto.Value,
            Date = dto.Date,
            Comment = dto.Comment
        };

        var created = await _gradeRepository.AddAsync(grade);
        return MapToDto(created);
    }

    public async Task UpdateAsync(int id, GradeUpdateDto dto)
    {
        var grade = await _gradeRepository.GetByIdAsync(id);
        if (grade == null) throw new KeyNotFoundException("Оценка не найдена");

        if (!await _studentRepository.ExistsAsync(dto.StudentId))
            throw new ArgumentException($"Студент с id={dto.StudentId} не найден");

        if (!await _subjectRepository.ExistsAsync(dto.SubjectId))
            throw new ArgumentException($"Предмет с id={dto.SubjectId} не найден");

        grade.StudentId = dto.StudentId;
        grade.SubjectId = dto.SubjectId;
        grade.Value = dto.Value;
        grade.Date = dto.Date;
        grade.Comment = dto.Comment;
        await _gradeRepository.UpdateAsync(grade);
    }

    public async Task DeleteAsync(int id)
    {
        var grade = await _gradeRepository.GetByIdAsync(id);
        if (grade == null) throw new KeyNotFoundException("Оценка не найдена");
        await _gradeRepository.DeleteAsync(grade);
    }

    public async Task<GroupAverageDto?> GetGroupAverageAsync(int groupId)
    {
        var group = await _groupRepository.GetByIdAsync(groupId);
        if (group == null) return null;

        var average = await _gradeRepository.GetAverageByGroupIdAsync(groupId);
        var grades = await _gradeRepository.GetByGroupIdAsync(groupId);

        return new GroupAverageDto
        {
            GroupId = group.Id,
            GroupName = group.Name,
            Course = group.Course,
            AverageGrade = Math.Round(average, 2),
            TotalGrades = grades.Count()
        };
    }

    private static GradeDto MapToDto(Grade grade)
    {
        return new GradeDto
        {
            Id = grade.Id,
            StudentId = grade.StudentId,
            StudentName = grade.Student?.FullName ?? string.Empty,
            SubjectId = grade.SubjectId,
            SubjectName = grade.Subject?.Name ?? string.Empty,
            Value = grade.Value,
            Date = grade.Date,
            Comment = grade.Comment
        };
    }
}
