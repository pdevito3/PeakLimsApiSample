namespace PeakLims.UnitTests.UnitTests.Domain.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.Domain.Accessions;
using PeakLims.Domain.Accessions.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class CreateAccessionTests
{
    private readonly Faker _faker;

    public CreateAccessionTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_accession()
    {
        // Arrange + Act
        var accessionToCreate = new FakeAccessionForCreationDto().Generate();
        var fakeAccession = FakeAccession.Generate(accessionToCreate);

        // Assert
        fakeAccession.Status.Should().Be(accessionToCreate.Status);
        fakeAccession.PatientId.Should().Be(accessionToCreate.PatientId);
        fakeAccession.HealthcareOrganizationId.Should().Be(accessionToCreate.HealthcareOrganizationId);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeAccession = FakeAccession.Generate();

        // Assert
        fakeAccession.DomainEvents.Count.Should().Be(1);
        fakeAccession.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(AccessionCreated));
    }
}