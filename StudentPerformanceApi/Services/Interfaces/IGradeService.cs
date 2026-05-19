using StudentPerformanceApi.DTOs;

namespace StudentPerformanceApi.Services.Interfaces;

public interface IGradeService
{
    Task<IEnumerable<GradeDto>> GetAllAsync(int? studentId = null, int? subjectId = null);
    Task<GradeDto?> GetByIdAsync(int id);
    Task<GradeDto> CreateAsync(GradeCreateDto dto);
    Task UpdateAsync(int id, GradeUpdateDto dto);
    Task DeleteAsync(int id);
    Task<GroupAverageDto?> GetGroupAverageAsync(int groupId);
}
