using System;
using Domain.Entities;
using Web.DataContracts.Dto;

namespace DataSeeder;

public static class PatientGenerator
{
    public static IEnumerable<PatientDto> Generate(int count)
    {
        if (count < 1)
            throw new ArgumentException("Items count must be greater than 0");

        var genders = Enum.GetValues<Gender>();
        var patients = new List<PatientDto>(count);

        var endDate = DateTime.Now;
        var startDate = endDate.AddDays(-30);
        var datesDelta = endDate - startDate;

        for (int i = 0; i < count; i++)
        {
            var randomTimespan = new TimeSpan(Random.Shared.Next(0, (int)datesDelta.TotalDays + 1), 0, 0, 0);
            var birthdate = startDate + randomTimespan;

            var patient = new PatientDto
            {
                Name =
                {
                    Use = "official",
                    Family = "big fam",
                    Given = [
                        $"baby{i + 1}",
                        $"babovich"
                    ]
                },
                Birthdate = birthdate,
                Active = Random.Shared.Next(0, 2) == 1,
                Gender = (Gender)genders.GetValue(Random.Shared.Next(genders.Length))!,
            };

            patients.Add(patient);
        }

        return patients;
    }
}
