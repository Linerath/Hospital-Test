using Application.Persistence;
using Application.Services;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Setup;

public static class ServicesExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HospitalDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("DbConnection"));
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPatientService, PatientService>();
    }
}
