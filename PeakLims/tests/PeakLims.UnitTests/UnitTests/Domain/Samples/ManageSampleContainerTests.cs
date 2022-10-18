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
public class ManageSampleContainerTests
{
    private readonly Faker _faker;

    public ManageSampleContainerTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void given_container_must_be_able_to_contain_the_sample_type()
    {
        // Arrange
        var container = FakeContainer.Generate();
        var containerUsedForString = container.UsedFor.Value;
        var sampleToCreate = new FakeContainerlessSampleForCreationDto().Generate();
        sampleToCreate.Type = _faker.PickRandom(SampleType.ListNames().Where(x => !x.Equals(containerUsedForString)));
        
        // Act
        var act = () => Sample.Create(sampleToCreate, container);

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage($"A {container.Type} container is used to store {container.UsedFor.Value} samples, not {sampleToCreate.Type}.");
    }
    
    [Test]
    public void must_use_active_container()
    {
        // Arrange
        var container = FakeContainer.Generate();
        var containerUsedForString = container.UsedFor.Value;
        container.Deactivate();
        var sampleToCreate = new FakeContainerlessSampleForCreationDto().Generate();
        sampleToCreate.Type = container.UsedFor.Value;

        // Act
        var act = () => Sample.Create(sampleToCreate, container);

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage($"Only active containers can be added to a sample.");
    }
}