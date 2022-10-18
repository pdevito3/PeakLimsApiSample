namespace PeakLims.IntegrationTests.FeatureTests.Panels;

using PeakLims.SharedTestHelpers.Fakes.Panel;
using PeakLims.Domain.Panels.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using Domain.Panels.Services;
using Domain.TestOrders.Services;
using Domain.Tests.Services;
using Services;
using SharedTestHelpers.Fakes.Accession;
using SharedTestHelpers.Fakes.HealthcareOrganization;
using SharedTestHelpers.Fakes.Patient;
using SharedTestHelpers.Fakes.Test;
using static TestFixture;

public class RemoveTestFromPanelCommandTests : TestBase
{
    [Test]
    public async Task can_remove_test_from_panel()
    {
        // Arrange
        var test = new FakeTestBuilder()
            .WithRepository(GetService<ITestRepository>())
            .Activate()
            .Build();
        var panel = FakePanelBuilder
            .Initialize()
            .WithPanelRepository(GetService<IPanelRepository>())
            .WithTestOrderRepository(GetService<ITestOrderRepository>())
            .WithTest(test)
            .Build();
        await InsertAsync(panel);

        // Act
        var command = new RemoveTestFromPanel.Command(panel.Id, test.Id);
        await SendAsync(command);
        var panelFromDb = await ExecuteDbContextAsync(db => db.Panels
            .Include(x => x.Tests)
            .FirstOrDefaultAsync(p => p.Id == panel.Id));

        // Assert
        panelFromDb.Tests.Count.Should().Be(0);
    }
    
    [Test]
    public async Task can_not_remove_test_to_panel_if_panel_is_actively_used()
    {
        // Arrange
        var test = new FakeTestBuilder()
            .WithRepository(GetService<ITestRepository>())
            .Activate()
            .Build();
        var secondTest = new FakeTestBuilder()
            .WithRepository(GetService<ITestRepository>())
            .Activate()
            .Build();
        await InsertAsync(secondTest);
        var panel = FakePanelBuilder
            .Initialize()
            .WithPanelRepository(GetService<IPanelRepository>())
            .WithTestOrderRepository(GetService<ITestOrderRepository>())
            .Activate()
            .WithTest(test)
            .Build();

        var fakePatientOne = FakePatient.Generate(GetService<IDateTimeProvider>());
        await InsertAsync(fakePatientOne);
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate();
        await InsertAsync(fakeHealthcareOrganizationOne);
        var accession = FakeAccessionBuilder
            .Initialize()
            .WithTestRepository(GetService<ITestRepository>())
            // .WithPatient(fakePatientOne)
            // .WithHealthcareOrganization(fakeHealthcareOrganizationOne)
            .WithPanel(panel)
            .Build();
        await InsertAsync(accession);

        // Act
        var command = new RemoveTestFromPanel.Command(panel.Id, secondTest.Id);
        var act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}