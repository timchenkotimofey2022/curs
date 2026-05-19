using Microsoft.EntityFrameworkCore;
using StudentPerformanceApi.Auth;
using StudentPerformanceApi.Models;

namespace StudentPerformanceApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Groups
        modelBuilder.Entity<Group>().HasData(
            new Group { Id = 1, Name = "ИС-101", Course = 1 },
            new Group { Id = 2, Name = "ИС-102", Course = 1 },
            new Group { Id = 3, Name = "ИС-201", Course = 2 },
            new Group { Id = 4, Name = "ИС-202", Course = 2 },
            new Group { Id = 5, Name = "ИС-301", Course = 3 }
        );

        // Seed Teachers
        modelBuilder.Entity<Teacher>().HasData(
            new Teacher { Id = 1, FullName = "Иванов Иван Иванович", Email = "ivanov@college.ru" },
            new Teacher { Id = 2, FullName = "Петрова Мария Сергеевна", Email = "petrova@college.ru" },
            new Teacher { Id = 3, FullName = "Сидоров Алексей Викторович", Email = "sidorov@college.ru" },
            new Teacher { Id = 4, FullName = "Кузнецова Елена Дмитриевна", Email = "kuznetsova@college.ru" },
            new Teacher { Id = 5, FullName = "Смирнов Дмитрий Александрович", Email = "smirnov@college.ru" }
        );

        // Seed Subjects
        modelBuilder.Entity<Subject>().HasData(
            new Subject { Id = 1, Name = "Математика", TeacherId = 1 },
            new Subject { Id = 2, Name = "Программирование", TeacherId = 2 },
            new Subject { Id = 3, Name = "Базы данных", TeacherId = 3 },
            new Subject { Id = 4, Name = "Веб-разработка", TeacherId = 4 },
            new Subject { Id = 5, Name = "Английский язык", TeacherId = 5 }
        );

        // Seed Students (20 students)
        int studentId = 1;
        var students = new List<Student>
        {
            new Student { Id = studentId++, FullName = "Абрамов Сергей Павлович", Email = "abramov@student.ru", GroupId = 1 },
            new Student { Id = studentId++, FullName = "Борисова Анна Владимировна", Email = "borisova@student.ru", GroupId = 1 },
            new Student { Id = studentId++, FullName = "Васильев Павел Андреевич", Email = "vasiliev@student.ru", GroupId = 1 },
            new Student { Id = studentId++, FullName = "Григорьева Ольга Николаевна", Email = "grigorieva@student.ru", GroupId = 1 },
            new Student { Id = studentId++, FullName = "Дмитриев Андрей Сергеевич", Email = "dmitriev@student.ru", GroupId = 2 },
            new Student { Id = studentId++, FullName = "Егорова Виктория Дмитриевна", Email = "egorova@student.ru", GroupId = 2 },
            new Student { Id = studentId++, FullName = "Жуков Максим Игоревич", Email = "zhukov@student.ru", GroupId = 2 },
            new Student { Id = studentId++, FullName = "Зайцева Екатерина Алексеевна", Email = "zaitseva@student.ru", GroupId = 2 },
            new Student { Id = studentId++, FullName = "Ильин Артём Викторович", Email = "ilin@student.ru", GroupId = 3 },
            new Student { Id = studentId++, FullName = "Козлова Светлана Павловна", Email = "kozlova@student.ru", GroupId = 3 },
            new Student { Id = studentId++, FullName = "Лебедев Николай Андреевич", Email = "lebedev@student.ru", GroupId = 3 },
            new Student { Id = studentId++, FullName = "Морозова Татьяна Сергеевна", Email = "morozova@student.ru", GroupId = 3 },
            new Student { Id = studentId++, FullName = "Новиков Денис Валерьевич", Email = "novikov@student.ru", GroupId = 4 },
            new Student { Id = studentId++, FullName = "Орлова Юлия Андреевна", Email = "orlova@student.ru", GroupId = 4 },
            new Student { Id = studentId++, FullName = "Попов Игорь Сергеевич", Email = "popov@student.ru", GroupId = 4 },
            new Student { Id = studentId++, FullName = "Романова Марина Павловна", Email = "romanova@student.ru", GroupId = 4 },
            new Student { Id = studentId++, FullName = "Соколов Владимир Дмитриевич", Email = "sokolov@student.ru", GroupId = 5 },
            new Student { Id = studentId++, FullName = "Тихонова Анастасия Игоревна", Email = "tikhonova@student.ru", GroupId = 5 },
            new Student { Id = studentId++, FullName = "Федоров Алексей Павлович", Email = "fedorov@student.ru", GroupId = 5 },
            new Student { Id = studentId++, FullName = "Шестакова Полина Викторовна", Email = "shestakova@student.ru", GroupId = 5 }
        };
        modelBuilder.Entity<Student>().HasData(students);

        // Seed Grades
        var random = new Random(42);
        int gradeId = 1;
        var grades = new List<Grade>();

        foreach (var student in students)
        {
            for (int subjectId = 1; subjectId <= 5; subjectId++)
            {
                grades.Add(new Grade
                {
                    Id = gradeId++,
                    StudentId = student.Id,
                    SubjectId = subjectId,
                    Value = random.Next(2, 6),
                    Date = DateTime.UtcNow.AddDays(-random.Next(1, 60)),
                    Comment = "Оценка за контрольную работу"
                });
            }
        }

        modelBuilder.Entity<Grade>().HasData(grades);

        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                FullName = "Администратор",
                Email = "admin@college.ru",
                PasswordHash = PasswordHasher.Hash("admin123"),
                Role = "Admin"
            },
            new User
            {
                Id = 2,
                FullName = "Иванов Иван Иванович",
                Email = "ivanov@college.ru",
                PasswordHash = PasswordHasher.Hash("teacher123"),
                Role = "Teacher"
            },
            new User
            {
                Id = 3,
                FullName = "Абрамов Сергей Павлович",
                Email = "abramov@student.ru",
                PasswordHash = PasswordHasher.Hash("student123"),
                Role = "Student"
            }
        );
    }
}
