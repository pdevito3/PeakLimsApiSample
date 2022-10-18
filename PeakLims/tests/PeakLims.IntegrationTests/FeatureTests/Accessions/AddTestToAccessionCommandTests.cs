namespace PeakLims.IntegrationTests.FeatureTests.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.Domain.Accessions.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using Domain.Tests.Services;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using Services;
using SharedTestHelpers.Fakes.Test;

public class AddTestToAccessionCommandTests : TestBase
{
    [Test]
    public async Task can_add_test_to_accession()
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
        await InsertAsync(fakeTest);

        var fakeAccessionOne = FakeAccessionBuilder
            .Initialize()
            .WithTestRepository(GetService<ITestRepository>())
            // .WithPatient(fakePatientOne)
            // .WithHealthcareOrganization(fakeHealthcareOrganizationOne)
            .ExcludeTestOrders()
            .Build();
        await InsertAsync(fakeAccessionOne);

        // Act
        var command = new AddTestToAccession.Command(fakeAccessionOne.Id, fakeTest.Id);
        await SendAsync(command);
        var accession = await ExecuteDbContextAsync(db => db.Accessions
            .Include(x => x.TestOrders)
            .ThenInclude(x => x.Test)
            .FirstOrDefaultAsync(a => a.Id == fakeAccessionOne.Id));
        var testOrders = accession.TestOrders;

        // Assert
        testOrders.Count.Should().Be(1);
        testOrders.FirstOrDefault().Test.TestName.Should().Be(fakeTest.TestName);
    }
    [Test]
    public async Task can_add_test_to_accession_with_existing_test_orders()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(GetService<IDateTimeProvider>());
        await InsertAsync(fakePatientOne);
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate();
        await InsertAsync(fakeHealthcareOrganizationOne);
        var existingTest = new FakeTestBuilder()
            .Activate()
            .WithRepository(GetService<ITestRepository>())
            .Build();
        var fakeTest = new FakeTestBuilder()
            .Activate()
            .WithRepository(GetService<ITestRepository>())
            .Build();
        await InsertAsync(fakeTest);

        var fakeAccessionOne = FakeAccessionBuilder
            .Initialize()
            .WithTestRepository(GetService<ITestRepository>())
            // .WithPatient(fakePatientOne)
            // .WithHealthcareOrganization(fakeHealthcareOrganizationOne)
            .WithTest(existingTest)
            .Build();
        await InsertAsync(fakeAccessionOne);

        // Act
        var command = new AddTestToAccession.Command(fakeAccessionOne.Id, fakeTest.Id);
        await SendAsync(command);
        var accession = await ExecuteDbContextAsync(db => db.Accessions
            .Include(x => x.TestOrders)
            .ThenInclude(x => x.Test)
            .FirstOrDefaultAsync(a => a.Id == fakeAccessionOne.Id));
        var testOrders = accession.TestOrders;

        // Assert
        testOrders.Count.Should().Be(2);
    }
}