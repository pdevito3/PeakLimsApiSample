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
        fakeHealthcareOrganization.Email.Should().Be(updatedHealthcareOrganization.Email);
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