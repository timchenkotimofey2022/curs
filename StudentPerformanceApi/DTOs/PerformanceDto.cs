namespace StudentPerformanceApi.DTOs;

public class StudentPerformanceDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public List<SubjectGradeDto> SubjectGrades { get; set; } = new();
    public double AverageGrade { get; set; }
}

public class SubjectGradeDto
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int Value { get; set; }
    public DateTime Date { get; set; }
    public string? Comment { get; set; }
}

public class GroupAverageDto
{
    public int GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public int Course { get; set; }
    public double AverageGrade { get; set; }
    public int TotalGrades { get; set; }
}
