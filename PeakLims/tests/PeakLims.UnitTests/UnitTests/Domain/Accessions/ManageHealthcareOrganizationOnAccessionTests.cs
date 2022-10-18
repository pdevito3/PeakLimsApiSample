namespace PeakLims.UnitTests.UnitTests.Domain.Accessions;

using Bogus;
using FluentAssertions;
using NUnit.Framework;
using PeakLims.Domain.Accessions;
using SharedTestHelpers.Fakes.HealthcareOrganization;

[Parallelizable]
public class ManageHealthcareOrganizationOnAccessionTests
{
    private readonly Faker _faker;

    public ManageHealthcareOrganizationOnAccessionTests()
    {
        _faker = new Faker();
    }

    [Test]
    public void can_manage_healthcareOrganization()
    {
        // Arrange
        var accession = Accession.Create();
        var healthcareOrganization = FakeHealthcareOrganization.Generate();
        
        // Act - add
        accession.SetHealthcareOrganization(healthcareOrganization);

        // Assert - Add
        accession.HealthcareOrganization.Should().BeEquivalentTo(healthcareOrganization);
        accession.HealthcareOrganization.Id.Should().Be(healthcareOrganization.Id);
        
        // Act - remove
        accession.RemoveHealthcareOrganization();

        // Assert - Remove
        accession.HealthcareOrganization.Should().BeNull();
        accession.HealthcareOrganizationId.Should().BeNull();
    }
}