namespace PeakLims.IntegrationTests.FeatureTests.Containers;

using PeakLims.SharedTestHelpers.Fakes.Container;
using PeakLims.Domain.Containers.Dtos;
using SharedKernel.Exceptions;
using PeakLims.Domain.Containers.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;

public class UpdateContainerCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_container_in_db()
    {
        // Arrange
        var fakeContainerOne = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());
        var updatedContainerDto = new FakeContainerForUpdateDto().Generate();
        await InsertAsync(fakeContainerOne);

        var container = await ExecuteDbContextAsync(db => db.Containers
            .FirstOrDefaultAsync(c => c.Id == fakeContainerOne.Id));
        var id = container.Id;

        // Act
        var command = new UpdateContainer.Command(id, updatedContainerDto);
        await SendAsync(command);
        var updatedContainer = await ExecuteDbContextAsync(db => db.Containers.FirstOrDefaultAsync(c => c.Id == id));

        // Assert
        updatedContainer.Status.Should().Be(fakeContainerOne.Status);
        updatedContainer.Type.Should().Be(updatedContainerDto.Type);
    }
}