namespace PeakLims.IntegrationTests.FeatureTests.Accessions;

using System.Threading.Tasks;
using Bogus;
using Domain.Accessions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PeakLims.Domain.Accessions.Features;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using static TestFixture;

public class ManageOrganizationOnAccessionCommandTests : TestBase
{
    private readonly Faker _faker;

    public ManageOrganizationOnAccessionCommandTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public async Task can_manage_org()
    {
        // Arrange
        var org = FakeHealthcareOrganization.Generate();
        await InsertAsync(org);
        var accession = Accession.Create();
        await InsertAsync(accession);

        // Act - set
        var command = new SetAccessionHealthcareOrganization.Command(accession.Id, org.Id);
        await SendAsync(command);
        var accessionFromDb = await ExecuteDbContextAsync(db => db.Accessions
            .FirstOrDefaultAsync(x => x.Id == accession.Id));

        // Assert - set
        accessionFromDb.HealthcareOrganizationId.Should().Be(org.Id);

        // Act - remove
        var removeCommand = new RemoveAccessionHealthcareOrganization.Command(accession.Id);
        await SendAsync(removeCommand);
        accessionFromDb = await ExecuteDbContextAsync(db => db.Accessions
            .FirstOrDefaultAsync(x => x.Id == accession.Id));

        // Assert - remove
        accessionFromDb.HealthcareOrganizationId.Should().BeNull();
    }
}