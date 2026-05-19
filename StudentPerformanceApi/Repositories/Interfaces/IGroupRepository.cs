using StudentPerformanceApi.Models;

namespace StudentPerformanceApi.Repositories.Interfaces;

public interface IGroupRepository : IGenericRepository<Group>
{
    Task<Group?> GetByIdWithStudentsAsync(int id);
}
