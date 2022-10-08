namespace PeakLims.IntegrationTests.FeatureTests.HealthcareOrganizations;

using PeakLims.Domain.HealthcareOrganizations.Dtos;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using SharedKernel.Exceptions;
using PeakLims.Domain.HealthcareOrganizations.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;

public class HealthcareOrganizationListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_healthcareorganization_list()
    {
        // Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        var fakeHealthcareOrganizationTwo = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        var queryParameters = new HealthcareOrganizationParametersDto();

        await InsertAsync(fakeHealthcareOrganizationOne, fakeHealthcareOrganizationTwo);

        // Act
        var query = new GetHealthcareOrganizationList.Query(queryParameters);
        var healthcareOrganizations = await SendAsync(query);

        // Assert
        healthcareOrganizations.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}