namespace PeakLims.IntegrationTests.FeatureTests.Containers;

using PeakLims.Domain.Containers.Dtos;
using PeakLims.SharedTestHelpers.Fakes.Container;
using SharedKernel.Exceptions;
using PeakLims.Domain.Containers.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;

public class ContainerListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_container_list()
    {
        // Arrange
        var fakeContainerOne = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());
        var fakeContainerTwo = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());
        var queryParameters = new ContainerParametersDto();

        await InsertAsync(fakeContainerOne, fakeContainerTwo);

        // Act
        var query = new GetContainerList.Query(queryParameters);
        var containers = await SendAsync(query);

        // Assert
        containers.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}