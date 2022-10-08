namespace PeakLims.UnitTests.UnitTests.Domain.Samples;

using PeakLims.SharedTestHelpers.Fakes.Sample;
using PeakLims.Domain.Samples;
using PeakLims.Domain.Samples.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

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
        // Arrange + Act
        var sampleToCreate = new FakeSampleForCreationDto().Generate();
        var fakeSample = FakeSample.Generate(sampleToCreate);

        // Assert
        fakeSample.SampleNumber.Should().Be(sampleToCreate.SampleNumber);
        fakeSample.Status.Should().Be(sampleToCreate.Status);
        fakeSample.Type.Should().Be(sampleToCreate.Type);
        fakeSample.Quantity.Should().Be(sampleToCreate.Quantity);
        fakeSample.CollectionDate.Should().Be(sampleToCreate.CollectionDate);
        fakeSample.ReceivedDate.Should().Be(sampleToCreate.ReceivedDate);
        fakeSample.CollectionSite.Should().Be(sampleToCreate.CollectionSite);
        fakeSample.PatientId.Should().Be(sampleToCreate.PatientId);
        fakeSample.ParentSampleId.Should().Be(sampleToCreate.ParentSampleId);
        fakeSample.ContainerId.Should().Be(sampleToCreate.ContainerId);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeSample = FakeSample.Generate();

        // Assert
        fakeSample.DomainEvents.Count.Should().Be(1);
        fakeSample.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(SampleCreated));
    }
}