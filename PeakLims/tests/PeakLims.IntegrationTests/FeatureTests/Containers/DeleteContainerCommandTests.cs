namespace PeakLims.IntegrationTests.FeatureTests.Containers;

using PeakLims.SharedTestHelpers.Fakes.Container;
using PeakLims.Domain.Containers.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;

public class DeleteContainerCommandTests : TestBase
{
    [Test]
    public async Task can_delete_container_from_db()
    {
        // Arrange
        var fakeContainerOne = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());
        await InsertAsync(fakeContainerOne);
        var container = await ExecuteDbContextAsync(db => db.Containers
            .FirstOrDefaultAsync(c => c.Id == fakeContainerOne.Id));

        // Act
        var command = new DeleteContainer.Command(container.Id);
        await SendAsync(command);
        var containerResponse = await ExecuteDbContextAsync(db => db.Containers.CountAsync(c => c.Id == container.Id));

        // Assert
        containerResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_container_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteContainer.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_container_from_db()
    {
        // Arrange
        var fakeContainerOne = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());
        await InsertAsync(fakeContainerOne);
        var container = await ExecuteDbContextAsync(db => db.Containers
            .FirstOrDefaultAsync(c => c.Id == fakeContainerOne.Id));

        // Act
        var command = new DeleteContainer.Command(container.Id);
        await SendAsync(command);
        var deletedContainer = await ExecuteDbContextAsync(db => db.Containers
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == container.Id));

        // Assert
        deletedContainer?.IsDeleted.Should().BeTrue();
    }
}