namespace PeakLims.IntegrationTests.FeatureTests.Accessions;

using PeakLims.Domain.Accessions.Dtos;
using PeakLims.SharedTestHelpers.Fakes.Accession;
using SharedKernel.Exceptions;
using PeakLims.Domain.Accessions.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using Domain.Accessions;
using Domain.Accessions.DomainEvents;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using Services;

public class AccessionListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_accession_list()
    {
        // Arrange
        var fakeAccessionOne = Accession.Create();
        var fakeAccessionTwo = Accession.Create();
        var queryParameters = new AccessionParametersDto();

        await InsertAsync(fakeAccessionOne, fakeAccessionTwo);

        // Act
        var query = new GetAccessionList.Query(queryParameters);
        var accessions = await SendAsync(query);

        // Assert
        accessions.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}