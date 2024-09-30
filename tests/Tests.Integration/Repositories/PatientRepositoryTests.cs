using Domain.Entities;
using Infrastructure.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Tests.Integration.Setup;
using Web.Services;

namespace Tests.Integration.Repositories;

public class PatientRepositoryTests : IntegrationTestBase
{
    private static List<Patient> patients = new()
    {
        new Patient { Id = Guid.NewGuid(), Family = "f", Birthdate = new DateTime(2013, 1 ,1 ) },
        new Patient { Id = Guid.NewGuid(), Family = "f", Birthdate = new DateTime(2013, 1 ,2 ) },
        new Patient { Id = Guid.NewGuid(), Family = "f", Birthdate = new DateTime(2013, 1 ,3 ) },
        new Patient { Id = Guid.NewGuid(), Family = "f", Birthdate = new DateTime(2013, 1 ,4 ) },
        new Patient { Id = Guid.NewGuid(), Family = "f", Birthdate = new DateTime(2013, 1 ,5 ) },
    };

    public static TheoryData<string[], Guid[]> TestSearchData = new()
    {
        { ["eq2013-01-01"], [ patients[0].Id ] },
        { ["eq2013-01-04"], [ patients[3].Id ] },
        { ["eq2013-05-01"], [] },
        { ["ne2013-01-01"], [ patients[1].Id, patients[2].Id, patients[3].Id, patients[4].Id ] },
        { ["ne2013-01-01", "ne2013-01-03", "ne2013-01-04", "ne2013-01-05"], [ patients[1].Id ] },

        { ["lt2013-01-03"], [ patients[0].Id, patients[1].Id ] },
        { ["lt2013-01-01"], [] },

        { ["gt2013-01-03"], [ patients[3].Id, patients[4].Id ] },
        { ["gt2013-01-05"], [] },

        { ["gt2013-01-01", "lt2013-01-03"], [ patients[1].Id ] },

        { ["ge2013-01-04"], [ patients[3].Id, patients[4].Id ] },

        { ["le2013-01-02"], [ patients[0].Id, patients[1].Id ] },

        { ["2013-01-01"], [] },
        { ["eq"], [] },
        { [""], [] },
        { [null!], [] },
    };

    private readonly DateTimeSearchParameterParser dateTimeSearchParameterParser;

    public PatientRepositoryTests(IntegrationTestWebAppFactory factory)
    : base(factory)
    {
        dateTimeSearchParameterParser = scope.ServiceProvider.GetRequiredService<DateTimeSearchParameterParser>();
    }

    [Fact]
    public async Task GetById_ShouldReturnItem()
    {
        using var transaction = dbContext.Database.BeginTransaction();

        var patient = new Patient { Id = Guid.NewGuid(), Family = "fam", Birthdate = DateTime.Now };

        await AddItems(patient);

        var dbPatient = await unitOfWork.PatientRepository.GetByIdAsync(patient.Id);
        dbPatient.ShouldNotBeNull();

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task Create_ShouldAddItem()
    {
        using var transaction = dbContext.Database.BeginTransaction();

        var patient = new Patient { Id = Guid.NewGuid(), Family = "fam", Birthdate = DateTime.Now };

        await unitOfWork.PatientRepository.CreateAsync(patient);
        await unitOfWork.SaveChangesAsync();

        var patientDb = dbContext.Patients.FirstOrDefault(p => p.Id == patient.Id);
        patientDb.ShouldNotBeNull();

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task Create_ShouldNotAddItemWithSameId()
    {
        using var transaction = dbContext.Database.BeginTransaction();

        var firstPatient = new Patient { Id = Guid.NewGuid(), Family = "fam", Birthdate = DateTime.Now };

        await unitOfWork.PatientRepository.CreateAsync(firstPatient);
        await unitOfWork.SaveChangesAsync();

        var secondPatient = new Patient { Id = firstPatient.Id, Family = "fam", Birthdate = DateTime.Now };

        await Should.ThrowAsync<InvalidOperationException>(async () => await unitOfWork.PatientRepository.CreateAsync(secondPatient));

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task Update_ShouldUpdateItem()
    {
        using var transaction = dbContext.Database.BeginTransaction();

        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            Family = "fam",
            Birthdate = DateTime.Now,
            Active = false,
            Gender = Gender.Male,
            Given = ["1", "2"],
            Use = "use"
        };

        await AddItems(patient);

        var updatedPatient = new Patient
        {
            Id = patient.Id,
            Family = "fam upd",
            Birthdate = DateTime.Now.AddDays(-1),
            Active = true,
            Gender = Gender.Female,
            Given = ["3", "4"],
            Use = "use upd"
        };

        await unitOfWork.PatientRepository.UpdateAsync(updatedPatient);
        await unitOfWork.SaveChangesAsync();

        var patientDb = dbContext.Patients.FirstOrDefault(p => p.Id == patient.Id);
        patientDb.ShouldNotBeNull();
        patientDb.Family.ShouldBe(updatedPatient.Family);
        patientDb.Birthdate.ShouldBe(updatedPatient.Birthdate);
        patientDb.Active.ShouldBe(updatedPatient.Active);
        patientDb.Gender.ShouldBe(updatedPatient.Gender);
        patientDb.Given.ShouldBe(["3", "4"]);
        patientDb.Use.ShouldBe(updatedPatient.Use);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task Update_ShouldNotUpdateNonExistingItem()
    {
        using var transaction = dbContext.Database.BeginTransaction();

        var patient = new Patient { Id = Guid.NewGuid(), Family = "fam", Birthdate = DateTime.Now };

        await Should.ThrowAsync<ItemNotFoundException>(async () => await unitOfWork.PatientRepository.UpdateAsync(patient));

        await transaction.RollbackAsync();
    }

    [Theory, MemberData(nameof(TestSearchData))]
    public async Task SearchByBirthdate_ShouldWork(string[] dates, Guid[] expectedPatientsIds)
    {
        using var transaction = dbContext.Database.BeginTransaction();

        await AddItems(patients.ToArray());

        var datesParameters = dates.Select(dateTimeSearchParameterParser.TryParse).Where(d => d is not null);
        var foundPatients = await unitOfWork.PatientRepository.SearchByBirthdateAsync(datesParameters);

        foundPatients.Count().ShouldBe(expectedPatientsIds.Length);

        foreach (var patient in foundPatients)
            expectedPatientsIds.ShouldContain(patient.Id);

        await transaction.RollbackAsync();
    }
}
