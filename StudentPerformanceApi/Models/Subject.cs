using System.ComponentModel.DataAnnotations;

namespace StudentPerformanceApi.Models;

public class Subject
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;

    public List<Grade> Grades { get; set; } = new();
}
