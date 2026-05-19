namespace StudentPerformanceApi.DTOs;

public class GradeDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int Value { get; set; }
    public DateTime Date { get; set; }
    public string? Comment { get; set; }
}

public class GradeCreateDto
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public int Value { get; set; }
    public DateTime Date { get; set; }
    public string? Comment { get; set; }
}

public class GradeUpdateDto
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public int Value { get; set; }
    public DateTime Date { get; set; }
    public string? Comment { get; set; }
}
