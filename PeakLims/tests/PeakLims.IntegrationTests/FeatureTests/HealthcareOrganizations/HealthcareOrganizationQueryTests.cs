namespace PeakLims.IntegrationTests.FeatureTests.HealthcareOrganizations;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using PeakLims.Domain.HealthcareOrganizations.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;

public class HealthcareOrganizationQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_healthcareorganization_with_accurate_props()
    {
        // Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne);

        // Act
        var query = new GetHealthcareOrganization.Query(fakeHealthcareOrganizationOne.Id);
        var healthcareOrganization = await SendAsync(query);

        // Assert
        healthcareOrganization.Name.Should().Be(fakeHealthcareOrganizationOne.Name);
        healthcareOrganization.Email.Should().Be(fakeHealthcareOrganizationOne.Email);
    }

    [Test]
    public async Task get_healthcareorganization_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetHealthcareOrganization.Query(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}