namespace PeakLims.IntegrationTests.FeatureTests.Panels;

using PeakLims.Domain.Panels.Dtos;
using PeakLims.SharedTestHelpers.Fakes.Panel;
using SharedKernel.Exceptions;
using PeakLims.Domain.Panels.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;

public class PanelListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_panel_list()
    {
        // Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto().Generate());
        var fakePanelTwo = FakePanel.Generate(new FakePanelForCreationDto().Generate());
        var queryParameters = new PanelParametersDto();

        await InsertAsync(fakePanelOne, fakePanelTwo);

        // Act
        var query = new GetPanelList.Query(queryParameters);
        var panels = await SendAsync(query);

        // Assert
        panels.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}