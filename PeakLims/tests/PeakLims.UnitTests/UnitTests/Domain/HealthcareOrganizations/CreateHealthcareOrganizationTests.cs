namespace PeakLims.UnitTests.UnitTests.Domain.HealthcareOrganizations;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using PeakLims.Domain.HealthcareOrganizations;
using PeakLims.Domain.HealthcareOrganizations.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class CreateHealthcareOrganizationTests
{
    private readonly Faker _faker;

    public CreateHealthcareOrganizationTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_healthcareOrganization()
    {
        // Arrange + Act
        var healthcareOrganizationToCreate = new FakeHealthcareOrganizationForCreationDto().Generate();
        var fakeHealthcareOrganization = FakeHealthcareOrganization.Generate(healthcareOrganizationToCreate);

        // Assert
        fakeHealthcareOrganization.Name.Should().Be(healthcareOrganizationToCreate.Name);
        fakeHealthcareOrganization.Email.Should().Be(healthcareOrganizationToCreate.Email);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeHealthcareOrganization = FakeHealthcareOrganization.Generate();

        // Assert
        fakeHealthcareOrganization.DomainEvents.Count.Should().Be(1);
        fakeHealthcareOrganization.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(HealthcareOrganizationCreated));
    }
}