namespace PeakLims.IntegrationTests.FeatureTests.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.Domain.Accessions.Dtos;
using SharedKernel.Exceptions;
using PeakLims.Domain.Accessions.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using Domain.Accessions;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using Services;

public class SetAccessionHealthcareOrganizationCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_accession_in_db()
    {
        // Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne);

        var fakeAccessionOne = Accession.Create();
        await InsertAsync(fakeAccessionOne);

        // Act
        var command = new SetAccessionHealthcareOrganization.Command(fakeAccessionOne.Id, fakeHealthcareOrganizationOne.Id);
        await SendAsync(command);
        var updatedAccession = await ExecuteDbContextAsync(db => db.Accessions.FirstOrDefaultAsync(a => a.Id == fakeAccessionOne.Id));

        // Assert
        updatedAccession.HealthcareOrganizationId.Should().Be(fakeHealthcareOrganizationOne.Id);
    }
}