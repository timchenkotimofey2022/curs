using System.ComponentModel.DataAnnotations;

namespace StudentPerformanceApi.Models;

public class Grade
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;

    [Range(1, 5)]
    public int Value { get; set; }

    public DateTime Date { get; set; }

    [MaxLength(500)]
    public string? Comment { get; set; }
}
