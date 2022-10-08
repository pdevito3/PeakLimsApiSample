namespace PeakLims.IntegrationTests.FeatureTests.Containers;

using PeakLims.SharedTestHelpers.Fakes.Container;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using PeakLims.Domain.Containers.Features;
using static TestFixture;
using SharedKernel.Exceptions;

public class AddContainerCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_container_to_db()
    {
        // Arrange
        var fakeContainerOne = new FakeContainerForCreationDto().Generate();

        // Act
        var command = new AddContainer.Command(fakeContainerOne);
        var containerReturned = await SendAsync(command);
        var containerCreated = await ExecuteDbContextAsync(db => db.Containers
            .FirstOrDefaultAsync(c => c.Id == containerReturned.Id));

        // Assert
        containerReturned.ContainerNumber.Should().Be(fakeContainerOne.ContainerNumber);
        containerReturned.Status.Should().Be(fakeContainerOne.Status);
        containerReturned.Type.Should().Be(fakeContainerOne.Type);

        containerCreated.ContainerNumber.Should().Be(fakeContainerOne.ContainerNumber);
        containerCreated.Status.Should().Be(fakeContainerOne.Status);
        containerCreated.Type.Should().Be(fakeContainerOne.Type);
    }
}