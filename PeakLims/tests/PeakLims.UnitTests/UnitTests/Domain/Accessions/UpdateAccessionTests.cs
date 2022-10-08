namespace PeakLims.UnitTests.UnitTests.Domain.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.Domain.Accessions;
using PeakLims.Domain.Accessions.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class UpdateAccessionTests
{
    private readonly Faker _faker;

    public UpdateAccessionTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_accession()
    {
        // Arrange
        var fakeAccession = FakeAccession.Generate();
        var updatedAccession = new FakeAccessionForUpdateDto().Generate();
        
        // Act
        fakeAccession.Update(updatedAccession);

        // Assert
        fakeAccession.Status.Should().Be(updatedAccession.Status);
        fakeAccession.PatientId.Should().Be(updatedAccession.PatientId);
        fakeAccession.HealthcareOrganizationId.Should().Be(updatedAccession.HealthcareOrganizationId);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeAccession = FakeAccession.Generate();
        var updatedAccession = new FakeAccessionForUpdateDto().Generate();
        fakeAccession.DomainEvents.Clear();
        
        // Act
        fakeAccession.Update(updatedAccession);

        // Assert
        fakeAccession.DomainEvents.Count.Should().Be(1);
        fakeAccession.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(AccessionUpdated));
    }
}