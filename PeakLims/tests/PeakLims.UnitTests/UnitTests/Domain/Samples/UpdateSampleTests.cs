namespace PeakLims.UnitTests.UnitTests.Domain.Samples;

using PeakLims.SharedTestHelpers.Fakes.Sample;
using PeakLims.Domain.Samples;
using PeakLims.Domain.Samples.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class UpdateSampleTests
{
    private readonly Faker _faker;

    public UpdateSampleTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_sample()
    {
        // Arrange
        var fakeSample = FakeSample.Generate();
        var updatedSample = new FakeSampleForUpdateDto().Generate();
        
        // Act
        fakeSample.Update(updatedSample);

        // Assert
        fakeSample.SampleNumber.Should().Be(fakeSample.SampleNumber);
        fakeSample.Type.Value.Should().Be(updatedSample.Type);
        fakeSample.Quantity.Should().Be(updatedSample.Quantity);
        fakeSample.CollectionDate.Should().Be(updatedSample.CollectionDate);
        fakeSample.ReceivedDate.Should().Be(updatedSample.ReceivedDate);
        fakeSample.CollectionSite.Should().Be(updatedSample.CollectionSite);
        fakeSample.PatientId.Should().Be(updatedSample.PatientId);
        fakeSample.ParentSampleId.Should().Be(updatedSample.ParentSampleId);
        fakeSample.ContainerId.Should().Be(updatedSample.ContainerId);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeSample = FakeSample.Generate();
        var updatedSample = new FakeSampleForUpdateDto().Generate();
        fakeSample.DomainEvents.Clear();
        
        // Act
        fakeSample.Update(updatedSample);

        // Assert
        fakeSample.DomainEvents.Count.Should().Be(1);
        fakeSample.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(SampleUpdated));
    }
}