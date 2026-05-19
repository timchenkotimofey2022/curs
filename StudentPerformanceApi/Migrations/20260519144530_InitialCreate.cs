using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentPerformanceApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Course = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    GroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    TeacherId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grades_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Id", "Course", "Name" },
                values: new object[,]
                {
                    { 1, 1, "ИС-101" },
                    { 2, 1, "ИС-102" },
                    { 3, 2, "ИС-201" },
                    { 4, 2, "ИС-202" },
                    { 5, 3, "ИС-301" }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Email", "FullName" },
                values: new object[,]
                {
                    { 1, "ivanov@college.ru", "Иванов Иван Иванович" },
                    { 2, "petrova@college.ru", "Петрова Мария Сергеевна" },
                    { 3, "sidorov@college.ru", "Сидоров Алексей Викторович" },
                    { 4, "kuznetsova@college.ru", "Кузнецова Елена Дмитриевна" },
                    { 5, "smirnov@college.ru", "Смирнов Дмитрий Александрович" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { 1, "admin@college.ru", "Администратор", "$2a$11$ZRGnGv4.g8oEqG/C/D49VeMmXRe8PBYWlmoSLySVh05Bpgi1gVmMa", "Admin" },
                    { 2, "ivanov@college.ru", "Иванов Иван Иванович", "$2a$11$RlyDef0dBWX9ZP31VRBGf.FCiV7dHsH3ir3tul7q.UF0Qv0sWOg1W", "Teacher" },
                    { 3, "abramov@student.ru", "Абрамов Сергей Павлович", "$2a$11$zgOoNCXHOdzpo3QJl0ytP.ck8FcQlOb0Qv8tTmuLlW.ujl6ATVha6", "Student" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Email", "FullName", "GroupId" },
                values: new object[,]
                {
                    { 1, "abramov@student.ru", "Абрамов Сергей Павлович", 1 },
                    { 2, "borisova@student.ru", "Борисова Анна Владимировна", 1 },
                    { 3, "vasiliev@student.ru", "Васильев Павел Андреевич", 1 },
                    { 4, "grigorieva@student.ru", "Григорьева Ольга Николаевна", 1 },
                    { 5, "dmitriev@student.ru", "Дмитриев Андрей Сергеевич", 2 },
                    { 6, "egorova@student.ru", "Егорова Виктория Дмитриевна", 2 },
                    { 7, "zhukov@student.ru", "Жуков Максим Игоревич", 2 },
                    { 8, "zaitseva@student.ru", "Зайцева Екатерина Алексеевна", 2 },
                    { 9, "ilin@student.ru", "Ильин Артём Викторович", 3 },
                    { 10, "kozlova@student.ru", "Козлова Светлана Павловна", 3 },
                    { 11, "lebedev@student.ru", "Лебедев Николай Андреевич", 3 },
                    { 12, "morozova@student.ru", "Морозова Татьяна Сергеевна", 3 },
                    { 13, "novikov@student.ru", "Новиков Денис Валерьевич", 4 },
                    { 14, "orlova@student.ru", "Орлова Юлия Андреевна", 4 },
                    { 15, "popov@student.ru", "Попов Игорь Сергеевич", 4 },
                    { 16, "romanova@student.ru", "Романова Марина Павловна", 4 },
                    { 17, "sokolov@student.ru", "Соколов Владимир Дмитриевич", 5 },
                    { 18, "tikhonova@student.ru", "Тихонова Анастасия Игоревна", 5 },
                    { 19, "fedorov@student.ru", "Федоров Алексей Павлович", 5 },
                    { 20, "shestakova@student.ru", "Шестакова Полина Викторовна", 5 }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Name", "TeacherId" },
                values: new object[,]
                {
                    { 1, "Математика", 1 },
                    { 2, "Программирование", 2 },
                    { 3, "Базы данных", 3 },
                    { 4, "Веб-разработка", 4 },
                    { 5, "Английский язык", 5 }
                });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "Comment", "Date", "StudentId", "SubjectId", "Value" },
                values: new object[,]
                {
                    { 1, "Оценка за контрольную работу", new DateTime(2026, 5, 10, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2217), 1, 1, 4 },
                    { 2, "Оценка за контрольную работу", new DateTime(2026, 4, 18, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2235), 1, 2, 2 },
                    { 3, "Оценка за контрольную работу", new DateTime(2026, 5, 3, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2237), 1, 3, 2 },
                    { 4, "Оценка за контрольную работу", new DateTime(2026, 4, 18, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2239), 1, 4, 4 },
                    { 5, "Оценка за контрольную работу", new DateTime(2026, 4, 4, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2240), 1, 5, 2 },
                    { 6, "Оценка за контрольную работу", new DateTime(2026, 5, 3, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2244), 2, 1, 2 },
                    { 7, "Оценка за контрольную работу", new DateTime(2026, 4, 30, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2245), 2, 2, 4 },
                    { 8, "Оценка за контрольную работу", new DateTime(2026, 5, 3, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2246), 2, 3, 3 },
                    { 9, "Оценка за контрольную работу", new DateTime(2026, 5, 16, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2247), 2, 4, 4 },
                    { 10, "Оценка за контрольную работу", new DateTime(2026, 4, 14, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2250), 2, 5, 5 },
                    { 11, "Оценка за контрольную работу", new DateTime(2026, 5, 10, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2251), 3, 1, 3 },
                    { 12, "Оценка за контрольную работу", new DateTime(2026, 4, 7, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2252), 3, 2, 2 },
                    { 13, "Оценка за контрольную работу", new DateTime(2026, 4, 17, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2253), 3, 3, 5 },
                    { 14, "Оценка за контрольную работу", new DateTime(2026, 4, 7, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2254), 3, 4, 2 },
                    { 15, "Оценка за контрольную работу", new DateTime(2026, 3, 26, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2255), 3, 5, 2 },
                    { 16, "Оценка за контрольную работу", new DateTime(2026, 4, 18, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2265), 4, 1, 4 },
                    { 17, "Оценка за контрольную работу", new DateTime(2026, 5, 16, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2266), 4, 2, 2 },
                    { 18, "Оценка за контрольную работу", new DateTime(2026, 4, 14, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2269), 4, 3, 3 },
                    { 19, "Оценка за контрольную работу", new DateTime(2026, 5, 18, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2270), 4, 4, 5 },
                    { 20, "Оценка за контрольную работу", new DateTime(2026, 4, 1, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2271), 4, 5, 4 },
                    { 21, "Оценка за контрольную работу", new DateTime(2026, 4, 26, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2273), 5, 1, 4 },
                    { 22, "Оценка за контрольную работу", new DateTime(2026, 5, 17, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2274), 5, 2, 2 },
                    { 23, "Оценка за контрольную работу", new DateTime(2026, 4, 2, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2275), 5, 3, 2 },
                    { 24, "Оценка за контрольную работу", new DateTime(2026, 5, 12, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2276), 5, 4, 4 },
                    { 25, "Оценка за контрольную работу", new DateTime(2026, 4, 3, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2277), 5, 5, 2 },
                    { 26, "Оценка за контрольную работу", new DateTime(2026, 4, 10, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2278), 6, 1, 3 },
                    { 27, "Оценка за контрольную работу", new DateTime(2026, 5, 16, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2282), 6, 2, 2 },
                    { 28, "Оценка за контрольную работу", new DateTime(2026, 4, 18, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2284), 6, 3, 4 },
                    { 29, "Оценка за контрольную работу", new DateTime(2026, 4, 24, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2285), 6, 4, 2 },
                    { 30, "Оценка за контрольную работу", new DateTime(2026, 4, 11, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2286), 6, 5, 4 },
                    { 31, "Оценка за контрольную работу", new DateTime(2026, 5, 18, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2295), 7, 1, 2 },
                    { 32, "Оценка за контрольную работу", new DateTime(2026, 5, 3, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2296), 7, 2, 3 },
                    { 33, "Оценка за контрольную работу", new DateTime(2026, 4, 6, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2297), 7, 3, 2 },
                    { 34, "Оценка за контрольную работу", new DateTime(2026, 4, 21, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2300), 7, 4, 2 },
                    { 35, "Оценка за контрольную работу", new DateTime(2026, 4, 1, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2300), 7, 5, 2 },
                    { 36, "Оценка за контрольную работу", new DateTime(2026, 4, 18, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2302), 8, 1, 3 },
                    { 37, "Оценка за контрольную работу", new DateTime(2026, 5, 18, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2303), 8, 2, 3 },
                    { 38, "Оценка за контрольную работу", new DateTime(2026, 5, 18, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2304), 8, 3, 5 },
                    { 39, "Оценка за контрольную работу", new DateTime(2026, 5, 15, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2305), 8, 4, 2 },
                    { 40, "Оценка за контрольную работу", new DateTime(2026, 5, 16, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2305), 8, 5, 4 },
                    { 41, "Оценка за контрольную работу", new DateTime(2026, 3, 25, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2307), 9, 1, 5 },
                    { 42, "Оценка за контрольную работу", new DateTime(2026, 4, 26, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2308), 9, 2, 3 },
                    { 43, "Оценка за контрольную работу", new DateTime(2026, 5, 16, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2309), 9, 3, 4 },
                    { 44, "Оценка за контрольную работу", new DateTime(2026, 5, 10, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2309), 9, 4, 3 },
                    { 45, "Оценка за контрольную работу", new DateTime(2026, 4, 2, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2310), 9, 5, 3 },
                    { 46, "Оценка за контрольную работу", new DateTime(2026, 4, 25, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2312), 10, 1, 4 },
                    { 47, "Оценка за контрольную работу", new DateTime(2026, 3, 26, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2312), 10, 2, 3 },
                    { 48, "Оценка за контрольную работу", new DateTime(2026, 4, 5, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2313), 10, 3, 4 },
                    { 49, "Оценка за контрольную работу", new DateTime(2026, 3, 27, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2315), 10, 4, 2 },
                    { 50, "Оценка за контрольную работу", new DateTime(2026, 4, 29, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2315), 10, 5, 5 },
                    { 51, "Оценка за контрольную работу", new DateTime(2026, 5, 9, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2317), 11, 1, 4 },
                    { 52, "Оценка за контрольную работу", new DateTime(2026, 4, 22, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2318), 11, 2, 2 },
                    { 53, "Оценка за контрольную работу", new DateTime(2026, 4, 3, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2319), 11, 3, 3 },
                    { 54, "Оценка за контрольную работу", new DateTime(2026, 5, 11, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2320), 11, 4, 2 },
                    { 55, "Оценка за контрольную работу", new DateTime(2026, 4, 14, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2321), 11, 5, 2 },
                    { 56, "Оценка за контрольную работу", new DateTime(2026, 3, 21, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2322), 12, 1, 4 },
                    { 57, "Оценка за контрольную работу", new DateTime(2026, 4, 9, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2323), 12, 2, 5 },
                    { 58, "Оценка за контрольную работу", new DateTime(2026, 5, 1, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2324), 12, 3, 4 },
                    { 59, "Оценка за контрольную работу", new DateTime(2026, 3, 21, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2325), 12, 4, 4 },
                    { 60, "Оценка за контрольную работу", new DateTime(2026, 5, 17, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2325), 12, 5, 4 },
                    { 61, "Оценка за контрольную работу", new DateTime(2026, 3, 25, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2327), 13, 1, 3 },
                    { 62, "Оценка за контрольную работу", new DateTime(2026, 5, 6, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2328), 13, 2, 2 },
                    { 63, "Оценка за контрольную работу", new DateTime(2026, 5, 16, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2328), 13, 3, 3 },
                    { 64, "Оценка за контрольную работу", new DateTime(2026, 4, 14, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2329), 13, 4, 2 },
                    { 65, "Оценка за контрольную работу", new DateTime(2026, 5, 13, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2330), 13, 5, 3 },
                    { 66, "Оценка за контрольную работу", new DateTime(2026, 5, 11, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2334), 14, 1, 5 },
                    { 67, "Оценка за контрольную работу", new DateTime(2026, 4, 30, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2334), 14, 2, 2 },
                    { 68, "Оценка за контрольную работу", new DateTime(2026, 4, 4, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2335), 14, 3, 3 },
                    { 69, "Оценка за контрольную работу", new DateTime(2026, 5, 18, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2336), 14, 4, 5 },
                    { 70, "Оценка за контрольную работу", new DateTime(2026, 3, 30, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2337), 14, 5, 2 },
                    { 71, "Оценка за контрольную работу", new DateTime(2026, 5, 4, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2338), 15, 1, 5 },
                    { 72, "Оценка за контрольную работу", new DateTime(2026, 3, 30, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2339), 15, 2, 5 },
                    { 73, "Оценка за контрольную работу", new DateTime(2026, 4, 16, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2340), 15, 3, 3 },
                    { 74, "Оценка за контрольную работу", new DateTime(2026, 4, 12, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2341), 15, 4, 4 },
                    { 75, "Оценка за контрольную работу", new DateTime(2026, 5, 1, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2342), 15, 5, 2 },
                    { 76, "Оценка за контрольную работу", new DateTime(2026, 5, 17, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2343), 16, 1, 2 },
                    { 77, "Оценка за контрольную работу", new DateTime(2026, 3, 24, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2344), 16, 2, 2 },
                    { 78, "Оценка за контрольную работу", new DateTime(2026, 4, 2, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2345), 16, 3, 5 },
                    { 79, "Оценка за контрольную работу", new DateTime(2026, 3, 27, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2346), 16, 4, 2 },
                    { 80, "Оценка за контрольную работу", new DateTime(2026, 5, 1, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2347), 16, 5, 2 },
                    { 81, "Оценка за контрольную работу", new DateTime(2026, 4, 16, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2348), 17, 1, 4 },
                    { 82, "Оценка за контрольную работу", new DateTime(2026, 5, 11, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2349), 17, 2, 4 },
                    { 83, "Оценка за контрольную работу", new DateTime(2026, 4, 25, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2350), 17, 3, 4 },
                    { 84, "Оценка за контрольную работу", new DateTime(2026, 4, 22, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2351), 17, 4, 5 },
                    { 85, "Оценка за контрольную работу", new DateTime(2026, 3, 23, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2352), 17, 5, 3 },
                    { 86, "Оценка за контрольную работу", new DateTime(2026, 4, 16, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2395), 18, 1, 3 },
                    { 87, "Оценка за контрольную работу", new DateTime(2026, 4, 1, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2397), 18, 2, 5 },
                    { 88, "Оценка за контрольную работу", new DateTime(2026, 5, 12, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2398), 18, 3, 2 },
                    { 89, "Оценка за контрольную работу", new DateTime(2026, 5, 8, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2399), 18, 4, 2 },
                    { 90, "Оценка за контрольную работу", new DateTime(2026, 4, 6, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2400), 18, 5, 5 },
                    { 91, "Оценка за контрольную работу", new DateTime(2026, 4, 14, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2402), 19, 1, 3 },
                    { 92, "Оценка за контрольную работу", new DateTime(2026, 5, 11, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2403), 19, 2, 3 },
                    { 93, "Оценка за контрольную работу", new DateTime(2026, 3, 23, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2404), 19, 3, 5 },
                    { 94, "Оценка за контрольную работу", new DateTime(2026, 5, 8, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2405), 19, 4, 5 },
                    { 95, "Оценка за контрольную работу", new DateTime(2026, 4, 17, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2406), 19, 5, 3 },
                    { 96, "Оценка за контрольную работу", new DateTime(2026, 3, 24, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2407), 20, 1, 4 },
                    { 97, "Оценка за контрольную работу", new DateTime(2026, 4, 4, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2408), 20, 2, 5 },
                    { 98, "Оценка за контрольную работу", new DateTime(2026, 4, 28, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2409), 20, 3, 3 },
                    { 99, "Оценка за контрольную работу", new DateTime(2026, 4, 1, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2410), 20, 4, 4 },
                    { 100, "Оценка за контрольную работу", new DateTime(2026, 3, 27, 14, 45, 29, 612, DateTimeKind.Utc).AddTicks(2411), 20, 5, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grades_StudentId",
                table: "Grades",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_SubjectId",
                table: "Grades",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_GroupId",
                table: "Students",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subjects",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
