using System.Text.Json;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class HospitalDbContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }

    private JsonSerializerOptions jsonOptions = new();

    public HospitalDbContext(DbContextOptions<HospitalDbContext> options)
        : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<Enum>().HaveConversion<string>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.Property(p => p.Given).HasConversion(
                given => JsonSerializer.Serialize(given, jsonOptions),
                given => JsonSerializer.Deserialize<string[]>(given, jsonOptions)
            );
        });
    }
}
