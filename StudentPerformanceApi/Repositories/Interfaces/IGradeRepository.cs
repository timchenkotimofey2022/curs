using System.Linq.Expressions;
using StudentPerformanceApi.Models;

namespace StudentPerformanceApi.Repositories.Interfaces;

public interface IGradeRepository : IGenericRepository<Grade>
{
    Task<IEnumerable<Grade>> GetAllWithDetailsAsync();
    Task<IEnumerable<Grade>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<Grade>> GetBySubjectIdAsync(int subjectId);
    Task<IEnumerable<Grade>> GetByStudentAndSubjectIdAsync(int studentId, int subjectId);
    Task<IEnumerable<Grade>> GetByGroupIdAsync(int groupId);
    Task<double> GetAverageByGroupIdAsync(int groupId);
}
