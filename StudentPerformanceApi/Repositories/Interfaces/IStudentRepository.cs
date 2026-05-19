using StudentPerformanceApi.Models;

namespace StudentPerformanceApi.Repositories.Interfaces;

public interface IStudentRepository : IGenericRepository<Student>
{
    Task<Student?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Student>> GetByGroupIdAsync(int groupId);
}
