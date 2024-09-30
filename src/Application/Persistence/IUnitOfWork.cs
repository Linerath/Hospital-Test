using Application.Persistence.Repositories;

namespace Application.Persistence;

public interface IUnitOfWork
{
    IPatientRepository PatientRepository { get; }

    Task<int> SaveChangesAsync();
}
