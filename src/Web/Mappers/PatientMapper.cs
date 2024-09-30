using Domain.Entities;
using Riok.Mapperly.Abstractions;
using Web.DataContracts.Dto;

namespace Web.Mappers;

[Mapper]
public static partial class PatientMapper
{
    [MapNestedProperties(nameof(PatientDto.Name))]
    public static partial Patient ToPatientEntity(this PatientDto patient);

    [MapProperty([nameof(Patient.Id)], [nameof(PatientDto.Name), nameof(PatientDto.Name.Id)])]
    [MapProperty([nameof(Patient.Use)], [nameof(PatientDto.Name), nameof(PatientDto.Name.Use)])]
    [MapProperty([nameof(Patient.Family)], [nameof(PatientDto.Name), nameof(PatientDto.Name.Family)])]
    [MapProperty([nameof(Patient.Given)], [nameof(PatientDto.Name), nameof(PatientDto.Name.Given)])]
    public static partial PatientDto ToPatientDto(this Patient patient);

}
