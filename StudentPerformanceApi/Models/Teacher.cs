using System.ComponentModel.DataAnnotations;

namespace StudentPerformanceApi.Models;

public class Teacher
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    public List<Subject> Subjects { get; set; } = new();
}
