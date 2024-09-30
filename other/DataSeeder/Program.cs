using DataSeeder;
using Flurl;

const int PatientsCount = 100;
string endpointUrl = new Url("http://localhost:8080/api/patients");

Console.WriteLine("Press [Enter] to seed patients");
var keyInfo = Console.ReadKey();

while (keyInfo.Key != ConsoleKey.Enter)
    keyInfo = Console.ReadKey();

Console.WriteLine($"Generating {PatientsCount} patients...");
Console.WriteLine();

var patients = PatientGenerator.Generate(PatientsCount);

Console.WriteLine($"Seeding patients...");
Console.WriteLine();

var patientApiSeeder = new PatientApiSeeder(endpointUrl);
await patientApiSeeder.SeedAsync(patients);

Console.WriteLine($"Done");
