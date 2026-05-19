using StudentPerformanceApi.Models;

namespace StudentPerformanceApi.Repositories.Interfaces;

public interface ISubjectRepository : IGenericRepository<Subject>
{
    Task<IEnumerable<Subject>> GetAllWithTeachersAsync();
    Task<IEnumerable<Subject>> GetByTeacherIdWithTeacherAsync(int teacherId);
}
