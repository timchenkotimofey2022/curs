using StudentPerformanceApi.DTOs;

namespace StudentPerformanceApi.Services.Interfaces;

public interface ISubjectService
{
    Task<IEnumerable<SubjectDto>> GetAllAsync(int? teacherId = null);
    Task<SubjectDto?> GetByIdAsync(int id);
    Task<SubjectDto> CreateAsync(SubjectCreateDto dto);
    Task UpdateAsync(int id, SubjectUpdateDto dto);
    Task DeleteAsync(int id);
}
