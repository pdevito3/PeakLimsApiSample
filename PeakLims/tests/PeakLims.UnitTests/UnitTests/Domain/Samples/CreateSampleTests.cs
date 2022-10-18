namespace PeakLims.UnitTests.UnitTests.Domain.Samples;

using PeakLims.SharedTestHelpers.Fakes.Sample;
using PeakLims.Domain.Samples;
using PeakLims.Domain.Samples.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using PeakLims.Domain.SampleTypes;
using SharedTestHelpers.Fakes.Container;

[Parallelizable]
public class CreateSampleTests
{
    private readonly Faker _faker;

    public CreateSampleTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_sample()
    {
        // Arrange
        var fakeContainer = FakeContainer.Generate();
        var sampleToCreate = new FakeContainerlessSampleForCreationDto().Generate();
        sampleToCreate.Type = fakeContainer.UsedFor.Value;
        
        // Act
        var fakeSample = Sample.Create(sampleToCreate, fakeContainer);

        // Assert
        fakeSample.Type.Value.Should().Be(sampleToCreate.Type);
        fakeSample.Quantity.Should().Be(sampleToCreate.Quantity);
        fakeSample.CollectionDate.Should().Be(sampleToCreate.CollectionDate);
        fakeSample.ReceivedDate.Should().Be(sampleToCreate.ReceivedDate);
        fakeSample.CollectionSite.Should().Be(sampleToCreate.CollectionSite);
        fakeSample.PatientId.Should().Be(sampleToCreate.PatientId);
        fakeSample.ParentSampleId.Should().Be(sampleToCreate.ParentSampleId);
        fakeSample.ContainerId.Should().Be(fakeContainer.Id);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeContainer = FakeContainer.Generate();
        var fakeSample = FakeSample.Generate(fakeContainer);

        // Assert
        fakeSample.DomainEvents.Count.Should().Be(1);
        fakeSample.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(SampleCreated));
    }
}