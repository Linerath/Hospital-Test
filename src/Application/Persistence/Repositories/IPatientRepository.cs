using Application.DataContracts;
using Domain.Entities;

namespace Application.Persistence.Repositories;

public interface IPatientRepository
{
    Task<Patient?> GetByIdAsync(Guid id);
    Task<Patient> CreateAsync(Patient patient);
    Task<Patient> UpdateAsync(Patient patient);
    void Delete(Patient patient);
    Task<IEnumerable<Patient>> SearchByBirthdateAsync(IEnumerable<DateTimeSearchParameter> searchParameters);
}
