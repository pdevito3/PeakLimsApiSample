namespace PeakLims.UnitTests.UnitTests.Domain.Containers;

using PeakLims.SharedTestHelpers.Fakes.Container;
using PeakLims.Domain.Containers;
using PeakLims.Domain.Containers.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class CreateContainerTests
{
    private readonly Faker _faker;

    public CreateContainerTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_container()
    {
        // Arrange + Act
        var containerToCreate = new FakeContainerForCreationDto().Generate();
        var fakeContainer = FakeContainer.Generate(containerToCreate);

        // Assert
        fakeContainer.ContainerNumber.Should().Be(containerToCreate.ContainerNumber);
        fakeContainer.State.Should().Be(containerToCreate.State);
        fakeContainer.Type.Should().Be(containerToCreate.Type);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeContainer = FakeContainer.Generate();

        // Assert
        fakeContainer.DomainEvents.Count.Should().Be(1);
        fakeContainer.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(ContainerCreated));
    }
}