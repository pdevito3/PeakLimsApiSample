namespace PeakLims.IntegrationTests.FeatureTests.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.Domain.Accessions.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using Domain.AccessionStatuses;
using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using SharedTestHelpers.Fakes.HealthcareOrganizationContact;
using SharedTestHelpers.Fakes.Panel;
using SharedTestHelpers.Fakes.PanelOrder;
using SharedTestHelpers.Fakes.Test;
using static TestFixture;

public class SetAccessionStatusToReadyForTestingCommandTests : TestBase
{
    [Test]
    public async Task can_change_status_to_readyfortesting()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto().Generate());
        await InsertAsync(fakePatientOne);
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne);
        var fakeAccessionOne = FakeAccession.Generate(new FakeAccessionForCreationDto()
            .RuleFor(a => a.PatientId, _ => fakePatientOne.Id)
            .RuleFor(a => a.HealthcareOrganizationId, _ => fakeHealthcareOrganizationOne.Id).Generate());
        
        var fakeContact = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(x => x.HealthcareOrganizationId, fakeHealthcareOrganizationOne.Id)
            .Generate());
        fakeAccessionOne.AddContact(fakeContact);
        
        var fakeTest = FakeTest.GenerateActivated();
        var fakePanel = FakePanel.Generate();
        fakePanel.AddTest(fakeTest);
        var fakePanelOrder = FakePanelOrder.Generate();
        fakePanelOrder.SetPanel(fakePanel);
        fakeAccessionOne.AddPanelOrder(fakePanelOrder);
        
        await InsertAsync(fakeAccessionOne);

        var accession = await ExecuteDbContextAsync(db => db.Accessions
            .FirstOrDefaultAsync(a => a.Id == fakeAccessionOne.Id));
        var id = accession.Id;

        // Act
        var command = new SetAccessionStatusToReadyForTesting.Command(id);
        await SendAsync(command);
        var updatedAccession = await ExecuteDbContextAsync(db => db.Accessions.FirstOrDefaultAsync(a => a.Id == id));

        // Assert
        updatedAccession?.Status.Should().Be(AccessionStatus.ReadyForTesting());
    }
}