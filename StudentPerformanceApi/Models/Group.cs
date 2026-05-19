using System.ComponentModel.DataAnnotations;

namespace StudentPerformanceApi.Models;

public class Group
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 6)]
    public int Course { get; set; }

    public List<Student> Students { get; set; } = new();
}
