using Application.DataContracts;
using Application.Persistence;
using Application.Services;
using Domain.Entities;
using Infrastructure.Exceptions;

namespace Infrastructure.Services;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork unitOfWork;

    public PatientService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public Task<Patient?> GetByIdAsync(Guid id) => unitOfWork.PatientRepository.GetByIdAsync(id);

    public async Task<Patient> CreateAsync(Patient patient)
    {
        var createdPatient = await unitOfWork.PatientRepository.CreateAsync(patient);
        await unitOfWork.SaveChangesAsync();

        return createdPatient;
    }

    public async Task<Patient> UpdateAsync(Patient patient)
    {
        var updatedPatient = await unitOfWork.PatientRepository.UpdateAsync(patient);
        await unitOfWork.SaveChangesAsync();

        return updatedPatient;
    }

    public async Task DeleteAsync(Guid id)
    {
        var patient = await GetByIdAsync(id);

        if (patient is null)
            throw new ItemNotFoundException();

        unitOfWork.PatientRepository.Delete(patient);
        await unitOfWork.SaveChangesAsync();
    }

    public Task<IEnumerable<Patient>> SearchByBirthdateAsync(IEnumerable<DateTimeSearchParameter> searchParameters) => unitOfWork.PatientRepository.SearchByBirthdateAsync(searchParameters);
}
