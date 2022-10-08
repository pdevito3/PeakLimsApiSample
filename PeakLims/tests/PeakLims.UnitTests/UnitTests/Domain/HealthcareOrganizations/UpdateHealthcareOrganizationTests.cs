namespace PeakLims.UnitTests.UnitTests.Domain.HealthcareOrganizations;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using PeakLims.Domain.HealthcareOrganizations;
using PeakLims.Domain.HealthcareOrganizations.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class UpdateHealthcareOrganizationTests
{
    private readonly Faker _faker;

    public UpdateHealthcareOrganizationTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_healthcareOrganization()
    {
        // Arrange
        var fakeHealthcareOrganization = FakeHealthcareOrganization.Generate();
        var updatedHealthcareOrganization = new FakeHealthcareOrganizationForUpdateDto().Generate();
        
        // Act
        fakeHealthcareOrganization.Update(updatedHealthcareOrganization);

        // Assert
        fakeHealthcareOrganization.Name.Should().Be(updatedHealthcareOrganization.Name);
        fakeHealthcareOrganization.Email.Value.Should().Be(updatedHealthcareOrganization.Email);
        fakeHealthcareOrganization.PrimaryAddress.Line1.Should().Be(updatedHealthcareOrganization.PrimaryAddress.Line1);
        fakeHealthcareOrganization.PrimaryAddress.Line2.Should().Be(updatedHealthcareOrganization.PrimaryAddress.Line2);
        fakeHealthcareOrganization.PrimaryAddress.City.Should().Be(updatedHealthcareOrganization.PrimaryAddress.City);
        fakeHealthcareOrganization.PrimaryAddress.State.Should().Be(updatedHealthcareOrganization.PrimaryAddress.State);
        fakeHealthcareOrganization.PrimaryAddress.PostalCode.Value.Should().Be(updatedHealthcareOrganization.PrimaryAddress.PostalCode);
        fakeHealthcareOrganization.PrimaryAddress.Country.Should().Be(updatedHealthcareOrganization.PrimaryAddress.Country);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeHealthcareOrganization = FakeHealthcareOrganization.Generate();
        var updatedHealthcareOrganization = new FakeHealthcareOrganizationForUpdateDto().Generate();
        fakeHealthcareOrganization.DomainEvents.Clear();
        
        // Act
        fakeHealthcareOrganization.Update(updatedHealthcareOrganization);

        // Assert
        fakeHealthcareOrganization.DomainEvents.Count.Should().Be(1);
        fakeHealthcareOrganization.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(HealthcareOrganizationUpdated));
    }
}