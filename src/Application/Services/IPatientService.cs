using Application.DataContracts;
using Domain.Entities;

namespace Application.Services;

public interface IPatientService
{
    Task<Patient?> GetByIdAsync(Guid id);
    Task<Patient> CreateAsync(Patient patient);
    Task<Patient> UpdateAsync(Patient patient);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Patient>> SearchByBirthdateAsync(IEnumerable<DateTimeSearchParameter> searchParameters);
}
