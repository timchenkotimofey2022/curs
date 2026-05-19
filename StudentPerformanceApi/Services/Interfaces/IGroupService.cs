using StudentPerformanceApi.DTOs;

namespace StudentPerformanceApi.Services.Interfaces;

public interface IGroupService
{
    Task<IEnumerable<GroupDto>> GetAllAsync();
    Task<GroupDto?> GetByIdAsync(int id);
    Task<GroupDto> CreateAsync(GroupCreateDto dto);
    Task UpdateAsync(int id, GroupUpdateDto dto);
    Task DeleteAsync(int id);
}
