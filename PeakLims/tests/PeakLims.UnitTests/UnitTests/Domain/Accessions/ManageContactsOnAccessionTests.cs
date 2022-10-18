namespace PeakLims.UnitTests.UnitTests.Domain.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using Bogus;
using FluentAssertions;
using NUnit.Framework;
using PeakLims.Domain.Accessions;
using SharedTestHelpers.Fakes.HealthcareOrganizationContact;

[Parallelizable]
public class ManageContactsOnAccessionTests
{
    private readonly Faker _faker;

    public ManageContactsOnAccessionTests()
    {
        _faker = new Faker();
    }

    [Test]
    public void can_manage_contact()
    {
        // Arrange
        var fakeAccession = Accession.Create();
        var contact = FakeHealthcareOrganizationContact.Generate();
        
        // Act - Can add idempotently
        fakeAccession.AddContact(contact)
            .AddContact(contact)
            .AddContact(contact);

        // Assert - Add
        fakeAccession.Contacts.Count.Should().Be(1);
        fakeAccession.Contacts.Should().ContainEquivalentOf(contact);
        
        // Act - Can remove idempotently
        fakeAccession.RemoveContact(contact)
            .RemoveContact(contact)
            .RemoveContact(contact);

        // Assert - Remove
        fakeAccession.Contacts.Count.Should().Be(0);
    }
}