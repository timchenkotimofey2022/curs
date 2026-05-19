using System.ComponentModel.DataAnnotations;

namespace StudentPerformanceApi.Models;

public class Student
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    public List<Grade> Grades { get; set; } = new();
}
