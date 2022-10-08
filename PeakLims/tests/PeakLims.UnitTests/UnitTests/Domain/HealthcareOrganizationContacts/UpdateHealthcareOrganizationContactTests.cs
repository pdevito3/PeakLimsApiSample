namespace PeakLims.UnitTests.UnitTests.Domain.HealthcareOrganizationContacts;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganizationContact;
using PeakLims.Domain.HealthcareOrganizationContacts;
using PeakLims.Domain.HealthcareOrganizationContacts.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class UpdateHealthcareOrganizationContactTests
{
    private readonly Faker _faker;

    public UpdateHealthcareOrganizationContactTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_healthcareOrganizationContact()
    {
        // Arrange
        var fakeHealthcareOrganizationContact = FakeHealthcareOrganizationContact.Generate();
        var updatedHealthcareOrganizationContact = new FakeHealthcareOrganizationContactForUpdateDto().Generate();
        
        // Act
        fakeHealthcareOrganizationContact.Update(updatedHealthcareOrganizationContact);

        // Assert
        fakeHealthcareOrganizationContact.Name.Should().Be(updatedHealthcareOrganizationContact.Name);
        fakeHealthcareOrganizationContact.Email.Should().Be(updatedHealthcareOrganizationContact.Email);
        fakeHealthcareOrganizationContact.Npi.Should().Be(updatedHealthcareOrganizationContact.Npi);
        fakeHealthcareOrganizationContact.HealthcareOrganizationId.Should().Be(updatedHealthcareOrganizationContact.HealthcareOrganizationId);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeHealthcareOrganizationContact = FakeHealthcareOrganizationContact.Generate();
        var updatedHealthcareOrganizationContact = new FakeHealthcareOrganizationContactForUpdateDto().Generate();
        fakeHealthcareOrganizationContact.DomainEvents.Clear();
        
        // Act
        fakeHealthcareOrganizationContact.Update(updatedHealthcareOrganizationContact);

        // Assert
        fakeHealthcareOrganizationContact.DomainEvents.Count.Should().Be(1);
        fakeHealthcareOrganizationContact.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(HealthcareOrganizationContactUpdated));
    }
}