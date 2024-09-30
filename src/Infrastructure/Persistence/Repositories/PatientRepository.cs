using Application.DataContracts;
using Application.Persistence.Repositories;
using Domain.Entities;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class PatientRepository : IPatientRepository
{
    private HospitalDbContext dbContext;

    public PatientRepository(HospitalDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Patient?> GetByIdAsync(Guid id) => await dbContext.Patients.FindAsync(id);

    public async Task<Patient> CreateAsync(Patient patient)
    {
        await dbContext.Patients.AddAsync(patient);
        return patient;
    }

    public async Task<Patient> UpdateAsync(Patient patient)
    {
        var dbPatient = await GetByIdAsync(patient.Id);

        if (dbPatient is null)
            throw new ItemNotFoundException();

        dbPatient.Use = patient.Use;
        dbPatient.Family = patient.Family;
        dbPatient.Given = patient.Given;
        dbPatient.Gender = patient.Gender;
        dbPatient.Birthdate = patient.Birthdate;
        dbPatient.Active = patient.Active;

        return dbPatient;
    }

    public void Delete(Patient patient) => dbContext.Patients.Remove(patient);

    public async Task<IEnumerable<Patient>> SearchByBirthdateAsync(IEnumerable<DateTimeSearchParameter> searchParameters)
    {
        if (!searchParameters.Any())
            return Enumerable.Empty<Patient>();

        var patientsQuery = dbContext.Patients.AsQueryable();

        foreach (var searchParam in searchParameters)
        {
            var date = searchParam.Date;
            patientsQuery = searchParam.Operator switch
            {
                DateTimeSearchOperator.Equal => patientsQuery.Where(p =>
                    p.Birthdate.Year == date.Year && p.Birthdate.Month == date.Month && p.Birthdate.Day == date.Day),
                DateTimeSearchOperator.NotEqual => patientsQuery.Where(p =>
                    !(p.Birthdate.Year == date.Year && p.Birthdate.Month == date.Month && p.Birthdate.Day == date.Day)),
                DateTimeSearchOperator.LessThan => patientsQuery.Where(p => p.Birthdate < date),
                DateTimeSearchOperator.GreaterThan => patientsQuery.Where(p => p.Birthdate > date),
                DateTimeSearchOperator.GreaterOrEqual => patientsQuery.Where(p => p.Birthdate >= date),
                DateTimeSearchOperator.LessOrEqual => patientsQuery.Where(p => p.Birthdate <= date),
                _ => throw new InvalidOperationException(),
            };
        }

        var patients = await patientsQuery.ToListAsync();

        return patients;
    }
}
