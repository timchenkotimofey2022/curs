using Microsoft.EntityFrameworkCore;
using StudentPerformanceApi.Data;
using StudentPerformanceApi.Models;
using StudentPerformanceApi.Repositories.Interfaces;

namespace StudentPerformanceApi.Repositories;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext context) : base(context) { }

    public override async Task<IEnumerable<Student>> GetAllAsync()
    {
        return await _dbSet
            .Include(s => s.Group)
            .ToListAsync();
    }

    public async Task<Student?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(s => s.Group)
            .Include(s => s.Grades)
            .ThenInclude(g => g.Subject)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Student>> GetByGroupIdAsync(int groupId)
    {
        return await _dbSet
            .Where(s => s.GroupId == groupId)
            .Include(s => s.Group)
            .ToListAsync();
    }
}
