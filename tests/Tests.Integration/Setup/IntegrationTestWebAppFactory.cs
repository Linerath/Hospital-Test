using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace Tests.Integration.Setup;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer dbContainer = new MsSqlBuilder()
        .WithCleanUp(true)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var dbContextDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(DbContextOptions<HospitalDbContext>));

            if (dbContextDescriptor is not null)
            {
                services.Remove(dbContextDescriptor);
            }

            services.AddDbContext<HospitalDbContext>(options =>
            {
                options.UseSqlServer(dbContainer.GetConnectionString());
            });
        });
    }

    public Task InitializeAsync() => dbContainer.StartAsync();

    public new Task DisposeAsync() => dbContainer.StopAsync();
}