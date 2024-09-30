using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Patient
{
    public Guid Id { get; set; }

    [MaxLength(256)]
    public string? Use { get; set; }

    [Required]
    [MaxLength(256)]
    public required string Family { get; set; }

    public string[]? Given { get; set; }

    [MaxLength(32)]
    public Gender Gender { get; set; }

    [Required]
    public required DateTime Birthdate { get; set; }

    public bool Active { get; set; }
}

public enum Gender
{
    Unknown,
    Male,
    Female,
    Other,
}
