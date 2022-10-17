namespace PeakLims.IntegrationTests.FeatureTests.Containers;

using PeakLims.SharedTestHelpers.Fakes.Container;
using PeakLims.Domain.Containers.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;

public class ContainerQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_container_with_accurate_props()
    {
        // Arrange
        var fakeContainerOne = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());
        await InsertAsync(fakeContainerOne);

        // Act
        var query = new GetContainer.Query(fakeContainerOne.Id);
        var container = await SendAsync(query);

        // Assert
        container.Status.Should().Be(fakeContainerOne.Status.Value);
        container.Type.Should().Be(fakeContainerOne.Type);
    }

    [Test]
    public async Task get_container_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetContainer.Query(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}