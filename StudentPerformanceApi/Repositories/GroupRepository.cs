using Microsoft.EntityFrameworkCore;
using StudentPerformanceApi.Data;
using StudentPerformanceApi.Models;
using StudentPerformanceApi.Repositories.Interfaces;

namespace StudentPerformanceApi.Repositories;

public class GroupRepository : GenericRepository<Group>, IGroupRepository
{
    public GroupRepository(ApplicationDbContext context) : base(context) { }

    public override async Task<IEnumerable<Group>> GetAllAsync()
    {
        return await _dbSet
            .Include(g => g.Students)
            .ToListAsync();
    }

    public async Task<Group?> GetByIdWithStudentsAsync(int id)
    {
        return await _dbSet
            .Include(g => g.Students)
            .FirstOrDefaultAsync(g => g.Id == id);
    }
}
