using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Web.DataContracts.Dto;

public class PatientDto
{
    [Required]
    public PatientNameDto Name { get; set; } = new();
    public Gender Gender { get; set; }
    [Required]
    public required DateTime Birthdate { get; set; }
    public bool Active { get; set; }
}

public class PatientNameDto
{
    public Guid Id { get; set; }
    public string? Use { get; set; }
    [Required]
    public string? Family { get; set; }
    public string[]? Given { get; set; }
}