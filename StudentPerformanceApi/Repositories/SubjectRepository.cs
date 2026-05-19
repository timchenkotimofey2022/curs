using Microsoft.EntityFrameworkCore;
using StudentPerformanceApi.Data;
using StudentPerformanceApi.Models;
using StudentPerformanceApi.Repositories.Interfaces;

namespace StudentPerformanceApi.Repositories;

public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
{
    public SubjectRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Subject>> GetAllWithTeachersAsync()
    {
        return await _dbSet.Include(s => s.Teacher).ToListAsync();
    }

    public async Task<IEnumerable<Subject>> GetByTeacherIdWithTeacherAsync(int teacherId)
    {
        return await _dbSet.Where(s => s.TeacherId == teacherId).Include(s => s.Teacher).ToListAsync();
    }
}
