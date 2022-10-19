namespace PeakLims.IntegrationTests.FeatureTests.Accessions;

using System.Threading.Tasks;
using Domain.Panels;
using Domain.TestOrders.Services;
using Domain.Tests.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PeakLims.Domain.Accessions.Features;
using PeakLims.Domain.Panels.Services;
using Services;
using SharedTestHelpers.Fakes.Accession;
using SharedTestHelpers.Fakes.HealthcareOrganization;
using SharedTestHelpers.Fakes.Panel;
using SharedTestHelpers.Fakes.Patient;
using SharedTestHelpers.Fakes.Test;
using static TestFixture;

public class AddPanelToAccessionCommandPanels : TestBase
{
    [Test]
    public async Task can_add_panel_to_accession()
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
        await InsertAsync(fakePanel);

        var fakeAccessionOne = FakeAccessionBuilder
            .Initialize()
            .WithTestRepository(GetService<ITestRepository>())
            .ExcludeTestOrders()
            .Build();
        await InsertAsync(fakeAccessionOne);

        // Act
        var command = new AddPanelToAccession.Command(fakeAccessionOne.Id, fakePanel.Id);
        await SendAsync(command);
        var accession = await ExecuteDbContextAsync(db => db.Accessions
            .Include(x => x.TestOrders)
            .ThenInclude(x => x.Test)
            .ThenInclude(x => x.Panels)
            .FirstOrDefaultAsync(a => a.Id == fakeAccessionOne.Id));
        var testOrders = accession.TestOrders;

        // Assert
        testOrders.Count.Should().Be(1);
        testOrders.FirstOrDefault().Test.TestName.Should().Be(fakePanel.Tests.FirstOrDefault().TestName);
    }
    [Test]
    public async Task can_add_panel_to_accession_with_existing_test_orders()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(GetService<IDateTimeProvider>());
        await InsertAsync(fakePatientOne);
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate();
        await InsertAsync(fakeHealthcareOrganizationOne);
        var existingText = new FakeTestBuilder()
            .Activate()
            .WithRepository(GetService<ITestRepository>())
            .Build();
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
        await InsertAsync(fakePanel);

        var fakeAccessionOne = FakeAccessionBuilder
            .Initialize()
            .WithTestRepository(GetService<ITestRepository>())
            // .WithPatient(fakePatientOne)
            // .WithHealthcareOrganization(fakeHealthcareOrganizationOne)
            .WithTest(existingText)
            .Build();
        await InsertAsync(fakeAccessionOne);

        // Act
        var command = new AddPanelToAccession.Command(fakeAccessionOne.Id, fakePanel.Id);
        await SendAsync(command);
        var accession = await ExecuteDbContextAsync(db => db.Accessions
            .Include(x => x.TestOrders)
            .ThenInclude(x => x.Test)
            .ThenInclude(x => x.Panels)
            .FirstOrDefaultAsync(a => a.Id == fakeAccessionOne.Id));
        var testOrders = accession.TestOrders;

        // Assert
        testOrders.Count.Should().Be(2);
    }
}