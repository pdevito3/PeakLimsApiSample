namespace PeakLims.UnitTests.UnitTests.Domain.Samples;

using PeakLims.SharedTestHelpers.Fakes.Sample;
using PeakLims.Domain.Samples;
using PeakLims.Domain.Samples.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using PeakLims.Domain.Containers;
using SharedTestHelpers.Fakes.Container;

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
        var fakeContainer = FakeContainer.Generate();
        var fakeSample = FakeSample.Generate(fakeContainer);
        var updatedSample = new FakeContainerlessSampleForUpdateDto().Generate();
        updatedSample.Type = fakeContainer.UsedFor.Value;
        
        // Act
        fakeSample.Update(updatedSample, fakeContainer);

        // Assert
        fakeSample.SampleNumber.Should().Be(fakeSample.SampleNumber);
        fakeSample.Type.Value.Should().Be(updatedSample.Type);
        fakeSample.Quantity.Should().Be(updatedSample.Quantity);
        fakeSample.CollectionDate.Should().Be(updatedSample.CollectionDate);
        fakeSample.ReceivedDate.Should().Be(updatedSample.ReceivedDate);
        fakeSample.CollectionSite.Should().Be(updatedSample.CollectionSite);
        fakeSample.PatientId.Should().Be(updatedSample.PatientId);
        fakeSample.ParentSampleId.Should().Be(updatedSample.ParentSampleId);
        fakeSample.ContainerId.Should().Be(fakeContainer.Id);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeContainer = FakeContainer.Generate();
        var fakeSample = FakeSample.Generate(fakeContainer);
        var updatedSample = new FakeContainerlessSampleForUpdateDto().Generate();
        updatedSample.Type = fakeContainer.UsedFor.Value;
        fakeSample.DomainEvents.Clear();
        
        // Act
        fakeSample.Update(updatedSample, fakeContainer);

        // Assert
        fakeSample.DomainEvents.Count.Should().Be(1);
        fakeSample.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(SampleUpdated));
    }
}