using Application.DataContracts;
using Application.Services;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Web.Binders;
using Web.DataContracts.Dto;
using Web.Mappers;

namespace Web.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService patientService;

    public PatientsController(IPatientService patientService)
    {
        this.patientService = patientService;
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<PatientDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [ModelBinder<PatientBirthdateSearchParameterBinder>]
        IEnumerable<DateTimeSearchParameter> searchParameters)
    {
        var patientsEntities = await patientService.SearchByBirthdateAsync(searchParameters);

        var patients = patientsEntities.Select(p => p.ToPatientDto());

        return Ok(patients);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid id)
    {
        var patientEntity = await patientService.GetByIdAsync(id);

        if (patientEntity is null)
            return NotFound();

        var patient = patientEntity.ToPatientDto();

        return Ok(patient);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(PatientDto patient)
    {
        var patientEntity = patient.ToPatientEntity();

        var createdPatientEntity = await patientService.CreateAsync(patientEntity);
        var createdPatient = createdPatientEntity.ToPatientDto();

        return StatusCode(StatusCodes.Status201Created, createdPatient);
    }

    [HttpPut]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(PatientDto patient)
    {
        var patientEntity = patient.ToPatientEntity();

        try
        {
            var updatedPatientEntity = await patientService.UpdateAsync(patientEntity);
            var updatedPatient = updatedPatientEntity.ToPatientDto();

            return Ok(updatedPatient);
        }
        catch (ItemNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await patientService.DeleteAsync(id);
            return Ok();
        }
        catch (ItemNotFoundException)
        {
            return NotFound();
        }
    }
}
