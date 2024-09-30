using Flurl;
using Flurl.Http;
using Web.DataContracts.Dto;
namespace DataSeeder;

public class PatientApiSeeder(Url endpointUrl)
{
    public async Task SeedAsync(IEnumerable<PatientDto> patients)
    {
        var tasks = new List<Task>();
        foreach (PatientDto patient in patients)
        {
            tasks.Add(CreatePatient(patient));
        }

        await Task.WhenAll(tasks);

        Console.WriteLine();
    }

    private async Task CreatePatient(PatientDto patient)
    {
        var patientName = string.Join(' ', patient.Name.Given ?? []);

        try
        {
            await endpointUrl.PostJsonAsync(patient);
            Console.WriteLine($"Successfully created {patientName}");
        }
        catch (FlurlHttpException ex)
        {
            var err = await ex.GetResponseStringAsync();
            Console.WriteLine($"Error creating {patientName}{Environment.NewLine}{err}");
        }
    }
}
