using Application.Persistence;
using Application.Persistence.Repositories;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure.Persistence;

internal class UnitOfWork : IUnitOfWork
{
    private readonly HospitalDbContext dbContext;

    public UnitOfWork(HospitalDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    private IPatientRepository? patientRepository;
    public IPatientRepository PatientRepository => patientRepository ??= new PatientRepository(dbContext);

    public Task<int> SaveChangesAsync() => dbContext.SaveChangesAsync();
}
