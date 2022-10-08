namespace PeakLims.UnitTests.UnitTests.Domain.HealthcareOrganizations;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using PeakLims.Domain.HealthcareOrganizations;
using PeakLims.Domain.HealthcareOrganizations.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using PeakLims.Domain.Emails;

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
        fakeHealthcareOrganization.Email.Should().Be(new Email(healthcareOrganizationToCreate.Email));
        fakeHealthcareOrganization.PrimaryAddress.Line1.Should().Be(healthcareOrganizationToCreate.PrimaryAddress.Line1);
        fakeHealthcareOrganization.PrimaryAddress.Line2.Should().Be(healthcareOrganizationToCreate.PrimaryAddress.Line2);
        fakeHealthcareOrganization.PrimaryAddress.City.Should().Be(healthcareOrganizationToCreate.PrimaryAddress.City);
        fakeHealthcareOrganization.PrimaryAddress.State.Should().Be(healthcareOrganizationToCreate.PrimaryAddress.State);
        fakeHealthcareOrganization.PrimaryAddress.PostalCode.Value.Should().Be(healthcareOrganizationToCreate.PrimaryAddress.PostalCode);
        fakeHealthcareOrganization.PrimaryAddress.Country.Should().Be(healthcareOrganizationToCreate.PrimaryAddress.Country);
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