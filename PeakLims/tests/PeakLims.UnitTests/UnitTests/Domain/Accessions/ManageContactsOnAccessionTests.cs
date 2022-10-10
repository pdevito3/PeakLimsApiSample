namespace PeakLims.UnitTests.UnitTests.Domain.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using Bogus;
using FluentAssertions;
using NUnit.Framework;
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
        var fakeAccession = FakeAccession.Generate();
        var contact = FakeHealthcareOrganizationContact.Generate();
        
        // Act - Add
        fakeAccession.AddContact(contact);
        
        // can idempotently add
        fakeAccession.AddContact(contact);
        fakeAccession.AddContact(contact);

        // Assert - Add
        fakeAccession.Contacts.Count.Should().Be(1);
        fakeAccession.Contacts.Should().ContainEquivalentOf(contact);
        
        // Act - Remove
        fakeAccession.RemoveContact(contact);
        
        // can idempotently remove
        fakeAccession.RemoveContact(contact);
        fakeAccession.RemoveContact(contact);

        // Assert - Remove
        fakeAccession.Contacts.Count.Should().Be(0);
    }
}