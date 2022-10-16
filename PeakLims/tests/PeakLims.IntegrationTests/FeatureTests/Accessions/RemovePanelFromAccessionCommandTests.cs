namespace PeakLims.IntegrationTests.FeatureTests.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.Domain.Accessions.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using Domain.Panels.Services;
using Domain.TestOrders.Services;
using Domain.Tests.Services;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using Services;
using SharedTestHelpers.Fakes.Panel;
using SharedTestHelpers.Fakes.Test;

public class RemovePanelFromAccessionCommandTests : TestBase
{
    [Test]
    public async Task can_remove_panel()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(GetService<IDateTimeProvider>());
        await InsertAsync(fakePatientOne);
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate();
        await InsertAsync(fakeHealthcareOrganizationOne);
        var fakeTest = new FakeTestBuilder()
            .Activate()
            .WithRepository(GetService<ITestRepository>())
            .Build();
        var fakePanel = FakePanelBuilder
            .Initialize()
            .WithPanelRepository(GetService<IPanelRepository>())
            .WithTestOrderRepository(GetService<ITestOrderRepository>())
            .Activate()
            .WithTest(fakeTest)
            .Build();

        var fakeAccessionOne = FakeAccessionBuilder
            .Initialize()
            .WithPatientId(fakePatientOne.Id)
            .WithHealthcareOrganizationId(fakeHealthcareOrganizationOne.Id)
            .WithTestRepository(GetService<ITestRepository>())
            .WithPanel(fakePanel)
            .Build();
        await InsertAsync(fakeAccessionOne);

        var testOrder = fakeAccessionOne.TestOrders.First();

        // Act
        var command = new RemovePanelFromAccession.Command(fakeAccessionOne.Id, fakePanel.Id);
        await SendAsync(command);
        var accession = await ExecuteDbContextAsync(db => db.Accessions
            .Include(x => x.TestOrders)
            .FirstOrDefaultAsync(a => a.Id == fakeAccessionOne.Id));
        var testOrderInDb = await ExecuteDbContextAsync(db => db.TestOrders
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(a => a.Id == testOrder.Id));
        var testOrders = accession.TestOrders;

        // Assert
        testOrders.Count.Should().Be(0);
        testOrderInDb.IsDeleted.Should().BeTrue();
    }
}