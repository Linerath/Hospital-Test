using Application.Persistence;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration.Setup;

public abstract class IntegrationTestBase : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly IServiceScope scope;
    protected readonly HospitalDbContext dbContext;
    protected readonly IUnitOfWork unitOfWork;

    public IntegrationTestBase(IntegrationTestWebAppFactory factory)
    {
        scope = factory.Services.CreateScope();
        dbContext = scope.ServiceProvider.GetRequiredService<HospitalDbContext>();
        unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }

    protected async Task AddItems(params object[] entities)
    {
        foreach (var entity in entities)
            await dbContext.AddAsync(entity);

        await dbContext.SaveChangesAsync();
    }
}