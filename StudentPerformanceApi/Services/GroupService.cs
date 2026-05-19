using StudentPerformanceApi.DTOs;
using StudentPerformanceApi.Models;
using StudentPerformanceApi.Repositories.Interfaces;
using StudentPerformanceApi.Services.Interfaces;

namespace StudentPerformanceApi.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;

    public GroupService(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<IEnumerable<GroupDto>> GetAllAsync()
    {
        var groups = await _groupRepository.GetAllAsync();
        return groups.Select(g => new GroupDto
        {
            Id = g.Id,
            Name = g.Name,
            Course = g.Course,
            StudentsCount = g.Students.Count
        });
    }

    public async Task<GroupDto?> GetByIdAsync(int id)
    {
        var group = await _groupRepository.GetByIdWithStudentsAsync(id);
        if (group == null) return null;

        return new GroupDto
        {
            Id = group.Id,
            Name = group.Name,
            Course = group.Course,
            StudentsCount = group.Students.Count
        };
    }

    public async Task<GroupDto> CreateAsync(GroupCreateDto dto)
    {
        var group = new Group
        {
            Name = dto.Name,
            Course = dto.Course
        };

        var created = await _groupRepository.AddAsync(group);
        return new GroupDto
        {
            Id = created.Id,
            Name = created.Name,
            Course = created.Course,
            StudentsCount = 0
        };
    }

    public async Task UpdateAsync(int id, GroupUpdateDto dto)
    {
        var group = await _groupRepository.GetByIdAsync(id);
        if (group == null) throw new KeyNotFoundException("Группа не найдена");

        group.Name = dto.Name;
        group.Course = dto.Course;
        await _groupRepository.UpdateAsync(group);
    }

    public async Task DeleteAsync(int id)
    {
        var group = await _groupRepository.GetByIdWithStudentsAsync(id);
        if (group == null) throw new KeyNotFoundException("Группа не найдена");

        if (group.Students.Any())
            throw new InvalidOperationException("Нельзя удалить группу, в которой есть студенты");

        await _groupRepository.DeleteAsync(group);
    }
}
