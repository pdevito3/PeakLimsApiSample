namespace PeakLims.UnitTests.UnitTests.Domain.HealthcareOrganizationContacts;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganizationContact;
using PeakLims.Domain.HealthcareOrganizationContacts;
using PeakLims.Domain.HealthcareOrganizationContacts.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using PeakLims.Domain.Emails;

[Parallelizable]
public class CreateHealthcareOrganizationContactTests
{
    private readonly Faker _faker;

    public CreateHealthcareOrganizationContactTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_healthcareOrganizationContact()
    {
        // Arrange + Act
        var healthcareOrganizationContactToCreate = new FakeHealthcareOrganizationContactForCreationDto().Generate();
        var fakeHealthcareOrganizationContact = FakeHealthcareOrganizationContact.Generate(healthcareOrganizationContactToCreate);

        // Assert
        fakeHealthcareOrganizationContact.Name.Should().Be(healthcareOrganizationContactToCreate.Name);
        fakeHealthcareOrganizationContact.Email.Should().Be(new Email(healthcareOrganizationContactToCreate.Email));
        fakeHealthcareOrganizationContact.Npi.Should().Be(healthcareOrganizationContactToCreate.Npi);
        fakeHealthcareOrganizationContact.HealthcareOrganizationId.Should().Be(healthcareOrganizationContactToCreate.HealthcareOrganizationId);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeHealthcareOrganizationContact = FakeHealthcareOrganizationContact.Generate();

        // Assert
        fakeHealthcareOrganizationContact.DomainEvents.Count.Should().Be(1);
        fakeHealthcareOrganizationContact.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(HealthcareOrganizationContactCreated));
    }
}