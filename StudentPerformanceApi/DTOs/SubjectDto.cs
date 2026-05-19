namespace StudentPerformanceApi.DTOs;

public class SubjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
}

public class SubjectCreateDto
{
    public string Name { get; set; } = string.Empty;
    public int TeacherId { get; set; }
}

public class SubjectUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public int TeacherId { get; set; }
}
