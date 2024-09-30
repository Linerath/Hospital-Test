using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Web.Setup;

public static class WebAppExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<HospitalDbContext>();

        dbContext.Database.Migrate();
    }
}