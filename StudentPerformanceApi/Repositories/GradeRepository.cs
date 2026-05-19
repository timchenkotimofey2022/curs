using Microsoft.EntityFrameworkCore;
using StudentPerformanceApi.Data;
using StudentPerformanceApi.Models;
using StudentPerformanceApi.Repositories.Interfaces;

namespace StudentPerformanceApi.Repositories;

public class GradeRepository : GenericRepository<Grade>, IGradeRepository
{
    public GradeRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Grade>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(g => g.Subject)
            .Include(g => g.Student)
            .ToListAsync();
    }

    public async Task<IEnumerable<Grade>> GetByStudentIdAsync(int studentId)
    {
        return await _dbSet
            .Where(g => g.StudentId == studentId)
            .Include(g => g.Subject)
            .Include(g => g.Student)
            .ToListAsync();
    }

    public async Task<IEnumerable<Grade>> GetBySubjectIdAsync(int subjectId)
    {
        return await _dbSet
            .Where(g => g.SubjectId == subjectId)
            .Include(g => g.Subject)
            .Include(g => g.Student)
            .ToListAsync();
    }

    public async Task<IEnumerable<Grade>> GetByStudentAndSubjectIdAsync(int studentId, int subjectId)
    {
        return await _dbSet
            .Where(g => g.StudentId == studentId && g.SubjectId == subjectId)
            .Include(g => g.Subject)
            .Include(g => g.Student)
            .ToListAsync();
    }

    public async Task<IEnumerable<Grade>> GetByGroupIdAsync(int groupId)
    {
        return await _dbSet
            .Where(g => g.Student.GroupId == groupId)
            .Include(g => g.Subject)
            .Include(g => g.Student)
            .ToListAsync();
    }

    public async Task<double> GetAverageByGroupIdAsync(int groupId)
    {
        var grades = await _dbSet
            .Where(g => g.Student.GroupId == groupId)
            .Select(g => g.Value)
            .ToListAsync();

        return grades.Any() ? grades.Average() : 0;
    }
}
