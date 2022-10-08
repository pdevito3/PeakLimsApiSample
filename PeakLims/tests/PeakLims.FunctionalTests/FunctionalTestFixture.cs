namespace PeakLims.FunctionalTests;

using Databases;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using DotNet.Testcontainers.Containers.Modules.Abstractions;
using DotNet.Testcontainers.Containers.Modules.Databases;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

[SetUpFixture]
public class FunctionalTestFixture
{
    public static IServiceScopeFactory ScopeFactory { get; private set; }
    public static WebApplicationFactory<Program> Factory  { get; private set; }
    private readonly TestcontainerDatabase _dbContainer = dbSetup();

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await _dbContainer.StartAsync();
        Environment.SetEnvironmentVariable("DB_CONNECTION_STRING", _dbContainer.ConnectionString);
        
        Factory = new TestingWebApplicationFactory();
        ScopeFactory = Factory.Services.GetRequiredService<IServiceScopeFactory>();

        using var scope = ScopeFactory.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<PeakLimsDbContext>();
        await db.Database.MigrateAsync();
    }
    
    [OneTimeTearDown]
    public async Task OneTimeTearDown()  
    {
        await _dbContainer.DisposeAsync();
        await Factory.DisposeAsync();
    }

    private static TestcontainerDatabase dbSetup()
    {
        return new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "db",
                Username = "postgres",
                Password = "postgres"
            })
            .WithName($"FunctionalTesting_PeakLims_{Guid.NewGuid()}")
            .WithImage("postgres:latest")
            .Build();
    }
}