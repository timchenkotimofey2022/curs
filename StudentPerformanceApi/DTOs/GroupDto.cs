namespace StudentPerformanceApi.DTOs;

public class GroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Course { get; set; }
    public int StudentsCount { get; set; }
}

public class GroupCreateDto
{
    public string Name { get; set; } = string.Empty;
    public int Course { get; set; }
}

public class GroupUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public int Course { get; set; }
}
