using StudentPerformanceApi.Data;
using StudentPerformanceApi.Models;
using StudentPerformanceApi.Repositories.Interfaces;

namespace StudentPerformanceApi.Repositories;

public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
{
    public TeacherRepository(ApplicationDbContext context) : base(context) { }
}
