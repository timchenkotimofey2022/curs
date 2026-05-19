using StudentPerformanceApi.DTOs;

namespace StudentPerformanceApi.Services.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllAsync(int? groupId = null);
    Task<StudentDto?> GetByIdAsync(int id);
    Task<StudentDto> CreateAsync(StudentCreateDto dto);
    Task UpdateAsync(int id, StudentUpdateDto dto);
    Task DeleteAsync(int id);
    Task<StudentPerformanceDto?> GetPerformanceAsync(int studentId);
}
