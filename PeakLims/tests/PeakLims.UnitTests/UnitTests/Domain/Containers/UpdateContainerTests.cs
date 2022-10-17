namespace PeakLims.UnitTests.UnitTests.Domain.Containers;

using PeakLims.SharedTestHelpers.Fakes.Container;
using PeakLims.Domain.Containers;
using PeakLims.Domain.Containers.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class UpdateContainerTests
{
    private readonly Faker _faker;

    public UpdateContainerTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_container()
    {
        // Arrange
        var fakeContainer = FakeContainer.Generate();
        var updatedContainer = new FakeContainerForUpdateDto().Generate();
        
        // Act
        fakeContainer.Update(updatedContainer);

        // Assert
        fakeContainer.Status.Should().Be(fakeContainer.Status);
        fakeContainer.Type.Should().Be(updatedContainer.Type);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeContainer = FakeContainer.Generate();
        var updatedContainer = new FakeContainerForUpdateDto().Generate();
        fakeContainer.DomainEvents.Clear();
        
        // Act
        fakeContainer.Update(updatedContainer);

        // Assert
        fakeContainer.DomainEvents.Count.Should().Be(1);
        fakeContainer.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(ContainerUpdated));
    }
}